CREATE TABLE [adm].[utbUserTokens]
(
	[Token]         VARCHAR (5000)  NOT NULL,
    [UserID]	    INT				NOT NULL,
    [ExpiresDate]	DATETIME	    NOT NULL,
    [CreationDate]  DATETIME        CONSTRAINT [utbUserTokensDefaultCreationDateSysDatetime] DEFAULT (sysdatetime()) NOT NULL,
    [CreationUser]  VARCHAR (100)   CONSTRAINT [utbUserTokensDefaultCreationUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbToken] PRIMARY KEY CLUSTERED ([Token]),
	CONSTRAINT [FK.adm.utbUsers.adm.utbUserTokens.UserID] FOREIGN KEY ([UserID]) REFERENCES [adm].[utbUsers] ([UserID])
);