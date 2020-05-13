CREATE TABLE [book].[utbAuditoriumSeats] (
    [SeatID]         VARCHAR (10)  NOT NULL,
    [RowID]          VARCHAR (10)  NOT NULL,
    [Label]          INT           NOT NULL,
    [Reserved]       BIT           NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbAuditoriumSeatsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME      CONSTRAINT [utbAuditoriumSeatsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100) CONSTRAINT [utbAuditoriumSeatsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbAuditoriumSeatsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbAuditoriumSeatsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbSeatID] PRIMARY KEY CLUSTERED ([SeatID] ASC),
    CONSTRAINT [FK.book.utbAuditoriumRows.book.utbAuditoriumSeats.RowID] FOREIGN KEY ([RowID]) REFERENCES [book].[utbAuditoriumRows] ([RowID])
);

