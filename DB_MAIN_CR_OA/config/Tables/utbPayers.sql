﻿CREATE TABLE [config].[utbPayers]
(
	[PrayerID]			INT				IDENTITY (1,1) NOT NULL,
    [Requester]			VARCHAR(100)	NOT NULL,
    [Email]				VARCHAR(50)		NOT NULL,
    [PhoneNumber]		VARCHAR(50)		NULL,
	[Reason]			VARCHAR(1000)	NOT NULL,
    [InsertDate]		DATETIME		CONSTRAINT [utbUsersDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
	[IP]				VARCHAR (20)	NULL,
    [Country]			VARCHAR (50)	NULL,
    [Region]			VARCHAR (50)	NULL,
    [City]				VARCHAR (50)	NULL,
    CONSTRAINT [utbPrayerID] PRIMARY KEY CLUSTERED ([PrayerID] ASC)
);