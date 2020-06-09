CREATE TABLE [config].[utbRegistrations]
(
	[RegisterID]    INT				IDENTITY (1, 1) NOT NULL,
    [UserID]		INT				NOT NULL,
	[FullName]		VARCHAR(100)	NOT NULL,
	[PhoneNumber]	VARCHAR(100)	NOT NULL,
    [EnrollmentID]	INT				NOT NULL,
	[ApprovalFlag]	BIT				CONSTRAINT [utbRegistrationsDefaultApprivalFlagIsFalse] DEFAULT ((0)) NOT NULL,
    [ActiveFlag]    BIT				CONSTRAINT [utbRegistrationsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [RegisterDate]	DATETIME		CONSTRAINT [utbRegistrationsDefaultInsertDateSYSDATETIME] DEFAULT (SYSDATETIME()) NOT NULL,
    [RegisterBy]	VARCHAR(100)	CONSTRAINT [utbRegistrationsRegisterBySUSER_SNAME] DEFAULT (SUSER_SNAME()) NOT NULL,
    CONSTRAINT [utbRegisterID] PRIMARY KEY CLUSTERED ([RegisterID] ASC),
	CONSTRAINT [FK.adm.utbUsers.config.utbRegistrations.UserID] FOREIGN KEY ([UserID]) REFERENCES [adm].[utbUsers] ([UserID]),
	CONSTRAINT [FK.config.utbEnrollments.config.utbRegistrations.GroupID] FOREIGN KEY ([EnrollmentID]) REFERENCES [config].[utbEnrollments] ([EnrollmentID])
);


GO
CREATE TRIGGER [config].[utrLogRegristrations] ON [config].[utbRegistrations]
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
				,'[config].[utbRegistrations]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[RegisterBy]
				,GETDATE()
		FROM Inserted 
	END