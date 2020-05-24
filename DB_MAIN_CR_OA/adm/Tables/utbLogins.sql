CREATE TABLE [adm].[utbLogins]
(
	[LoginID]	INT				IDENTITY (1, 1) NOT NULL,
    [UserID]	INT				NOT NULL,
    [IP]		VARCHAR (20)	NULL,
    [Country]   VARCHAR (50)	NULL,
    [Region]    VARCHAR (50)	NULL,
    [City]      VARCHAR (50)	NULL,
	[LoginDate]	DATETIME	  NOT NULL,
    CONSTRAINT [utbLoginID] PRIMARY KEY CLUSTERED ([LoginID] ASC),
	CONSTRAINT [FK.adm.utbUsers.adm.utbLogins.UserID] FOREIGN KEY ([UserID]) REFERENCES [adm].[utbUsers] ([UserID])
);