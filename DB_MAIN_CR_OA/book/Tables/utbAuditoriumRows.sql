CREATE TABLE [book].[utbAuditoriumRows] (
    [RowID]          VARCHAR (10)  NOT NULL,
    [BlockID]        VARCHAR (10)  NOT NULL,
    [Seats]          INT           NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbAuditoriumRowsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME      CONSTRAINT [utbAuditoriumRowsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100) CONSTRAINT [utbAuditoriumRowsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbAuditoriumRowsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbAuditoriumRowsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbRowID] PRIMARY KEY CLUSTERED ([RowID] ASC),
    CONSTRAINT [FK.book.utbAuditoriumLayout.book.utbAuditoriumRows.BlockID] FOREIGN KEY ([BlockID]) REFERENCES [book].[utbAuditoriumLayout] ([BlockID])
);

