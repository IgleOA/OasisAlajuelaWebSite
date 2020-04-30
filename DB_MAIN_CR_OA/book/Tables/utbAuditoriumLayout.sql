CREATE TABLE [book].[utbAuditoriumLayout] (
    [BlockID]        VARCHAR (10)  NOT NULL,
    [Rows]           INT           NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbAuditoriumLayoutDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME      CONSTRAINT [utbAuditoriumLayoutDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100) CONSTRAINT [utbAuditoriumLayoutDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbAuditoriumLayoutDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbAuditoriumLayoutDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbBlockID] PRIMARY KEY CLUSTERED ([BlockID] ASC)
);

