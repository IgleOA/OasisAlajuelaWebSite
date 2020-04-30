CREATE TABLE [book].[utbReservations] (
    [ReservationID]  INT           IDENTITY (1, 1) NOT NULL,
    [GUID]           VARCHAR (MAX) NOT NULL,
    [WorshipID]      INT           NOT NULL,
    [SeatID]         VARCHAR (10)  NOT NULL,
    [BookedBy]       INT           NOT NULL,
    [BookedFor]      VARCHAR (100) NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbReservationsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME      CONSTRAINT [utbReservationsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100) CONSTRAINT [utbReservationsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbReservationsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbReservationsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbReservationID] PRIMARY KEY CLUSTERED ([ReservationID] ASC),
    CONSTRAINT [FK.adm.utbUsers.book.utbReservations.BookedBy] FOREIGN KEY ([BookedBy]) REFERENCES [adm].[utbUsers] ([UserID]),
    CONSTRAINT [FK.book.utbAuditoriumSeats.book.utbReservations.RowID] FOREIGN KEY ([SeatID]) REFERENCES [book].[utbAuditoriumSeats] ([SeatID]),
    CONSTRAINT [FK.book.utbWorships.book.utbReservations.WorshipID] FOREIGN KEY ([WorshipID]) REFERENCES [book].[utbWorships] ([WorshipID])
);

