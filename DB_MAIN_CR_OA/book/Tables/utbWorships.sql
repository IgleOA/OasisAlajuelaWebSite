CREATE TABLE [book].[utbWorships] (
    [WorshipID]      INT           IDENTITY (1, 1) NOT NULL,
    [ScheduledDate]  DATETIME      NOT NULL,
    [Capacity]       INT           NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbWorshipsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME      CONSTRAINT [utbWorshipsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100) CONSTRAINT [utbWorshipsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbWorshipsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbWorshipsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbBookWorshipID] PRIMARY KEY CLUSTERED ([WorshipID] ASC)
);

