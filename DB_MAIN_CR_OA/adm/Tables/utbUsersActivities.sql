CREATE TABLE [adm].[utbUsersActivities] (
    [ActivityID]   INT           IDENTITY (1, 1) NOT NULL,
    [UserID]       INT           NOT NULL,
    [ActivityDate] DATETIME      NOT NULL,
    [Controller]   VARCHAR (50)  NOT NULL,
    [Action]       VARCHAR (50)  NOT NULL,
    [CreationDate] DATETIME      CONSTRAINT [utbUsersActivitiesDefaultCreationDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [CreationUser] VARCHAR (100) CONSTRAINT [utbUsersActivitiesDefaultCreationUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbActivityID] PRIMARY KEY CLUSTERED ([ActivityID] ASC),
    CONSTRAINT [FK.adm.utbUsers.adm.utbUsersActivities.UserID] FOREIGN KEY ([UserID]) REFERENCES [adm].[utbUsers] ([UserID])
);

