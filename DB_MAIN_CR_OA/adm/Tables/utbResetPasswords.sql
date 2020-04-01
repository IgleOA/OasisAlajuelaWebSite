CREATE TABLE [adm].[utbResetPasswords] (
    [RSID]           INT           IDENTITY (1, 1) NOT NULL,
    [GUID]           VARCHAR (MAX) NOT NULL,
    [UserID]         INT           NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbResetPasswordsDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME      CONSTRAINT [utbResetPasswordsDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100) CONSTRAINT [utbResetPasswordsDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbResetPasswordsDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbResetPasswordsDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbRSID] PRIMARY KEY CLUSTERED ([RSID] ASC),
    CONSTRAINT [fk.adm.utbUsers.adm.utbResetPasswords.UserID] FOREIGN KEY ([UserID]) REFERENCES [adm].[utbUsers] ([UserID])
);

