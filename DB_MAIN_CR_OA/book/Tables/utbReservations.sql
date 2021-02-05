CREATE TABLE [book].[utbReservations] (
    [ReservationID]  INT           IDENTITY (1, 1) NOT NULL,
    [GUID]           VARCHAR (MAX) NOT NULL,
    [EventID]		 INT           NOT NULL,
    [BookedBy]       INT           NOT NULL,
    [FirstName]      VARCHAR (100) NOT NULL,
    [LastName]       VARCHAR (100) NOT NULL,
    [IdentityID]     VARCHAR (100) NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbReservationsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME      CONSTRAINT [utbReservationsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100) CONSTRAINT [utbReservationsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbReservationsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbReservationsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbReservationID] PRIMARY KEY CLUSTERED ([ReservationID] ASC),
    CONSTRAINT [FK.adm.utbUsers.book.utbReservations.BookedBy] FOREIGN KEY ([BookedBy]) REFERENCES [adm].[utbUsers] ([UserID]),
    CONSTRAINT [FK.config.utbUpcomingEvents.book.utbReservations.EventID] FOREIGN KEY ([EventID]) REFERENCES [config].[utbUpcomingEvents] ([EventID])
);

