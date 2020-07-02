CREATE TABLE [config].[utbBlogs]
(
	[BlogID]	     INT             IDENTITY (1, 1) NOT NULL,
    [Title]          VARCHAR (30)    NOT NULL,
    [KeyWord]		 VARCHAR (30)    NOT NULL,
	[Description]    VARCHAR (200)   NOT NULL,
	[BannerPath]     VARCHAR (500)   NOT NULL,
	[MinisterID]     INT             NOT NULL,
    [ActiveFlag]     BIT             CONSTRAINT [utbBlogsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME        CONSTRAINT [utbBlogsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100)   CONSTRAINT [utbBlogsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME        CONSTRAINT [utbBlogsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100)   CONSTRAINT [utbBlogsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbBlogID] PRIMARY KEY CLUSTERED ([BlogID] ASC),
	CONSTRAINT [FK.config.utbMinisters.config.utbBlogs.MinisterID] FOREIGN KEY ([MinisterID]) REFERENCES [config].[utbMinisters] ([MinisterID])
);


GO

CREATE TRIGGER [config].[utrLogBlogs] ON [config].[utbBlogs]
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
				,'[config].[utbBlogs]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END
