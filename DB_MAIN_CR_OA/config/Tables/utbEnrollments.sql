CREATE TABLE [config].[utbEnrollments]
(
	[EnrollmentID]		INT				IDENTITY (1,1) NOT NULL,
    [GroupID]			INT				NOT NULL,
	[OpenRegister]		DATETIME		NOT NULL,
	[CloseRegister]		DATETIME		NOT NULL,
	[ApprovalFlag]		BIT				CONSTRAINT [utbEnrollmentsDefaultApprivalFlagIsFalse] DEFAULT ((0)) NOT NULL,
    [ActiveFlag]		BIT				CONSTRAINT [utbEnrollmentsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]		DATETIME		CONSTRAINT [utbEnrollmentsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbEnrollmentsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbEnrollmentsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbEnrollmentsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbEnrollmentID] PRIMARY KEY CLUSTERED ([EnrollmentID] ASC),
	CONSTRAINT [FK.config.utbGroups.config.utbEnrollments.GroupID] FOREIGN KEY ([GroupID]) REFERENCES [config].[utbGroups] ([GroupID])
);


GO
CREATE TRIGGER [config].[utrLogEnrollments] ON [config].[utbEnrollments]
FOR INSERT,UPDATE
AS
	BEGIN
		DECLARE @INSERTUPDATE VARCHAR(30)
		DECLARE @StartValues	XML = (SELECT * FROM Deleted [Values] for xml AUTO, ELEMENTS XSINIL)
		DECLARE @EndValues		XML = (SELECT * FROM Inserted [Values] for xml AUTO, ELEMENTS XSINIL)

		CREATE TABLE #DBCC (EventType varchar(50), Parameters varchar(50), EventInfo nvarchar(max))

		INSERT INTO #DBCC
		EXEC ('DBCC INPUTBUFFER(@@SPID)')

		--Assume it is an insert
		SET @INSERTUPDATE ='INSERT'
		--If there's data in deleted, it's an update
		IF EXISTS(SELECT * FROM Deleted)
		  SET @INSERTUPDATE='UPDATE'

		INSERT INTO [adm].[utbLogActivities] ([ActivityType],[TargetTable],[SQLStatement],[StartValues],[EndValues],[User],[LogActivityDate])
		SELECT	@INSERTUPDATE
				,'[config].[utbEnrollments]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END