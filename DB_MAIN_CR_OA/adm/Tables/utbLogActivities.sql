CREATE TABLE [adm].[utbLogActivities] (
    [ActivityID]      INT           IDENTITY (1, 1) NOT NULL,
    [ActivityType]    VARCHAR (50)  NOT NULL,
    [TargetTable]     VARCHAR (100) NOT NULL,
    [SQLStatement]    VARCHAR (MAX) NOT NULL,
    [StartValues]     XML           NULL,
    [EndValues]       XML           NOT NULL,
    [User]            VARCHAR (100) CONSTRAINT [utbLogActivitiesDefaultUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    [LogActivityDate] DATETIME      CONSTRAINT [utbLogActivitiesDefaultLogActivityDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [ubtActivityID] PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

