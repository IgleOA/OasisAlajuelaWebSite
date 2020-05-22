-- ======================================================================
-- Name: [config].[uspReadUserNotes]
-- Desc: Retorna las notas de un usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadUserNotes]
	@NoteID			INT = NULL,
	@UserName		VARCHAR(100) = NULL,
	@HistoryFlag	BIT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				IF(@NoteID IS NOT NULL)
					BEGIN
						SELECT	N.[NoteID]
										,N.[UserID]
										,N.[RequestNote]
										,[NoteDate]		= DATEADD(HOUR,-6,N.[InsertDate])
										,[InsertedBy]	= CASE WHEN R.[RoleName] LIKE '%Admin%' THEN 'Grupo Administrador' 
															   ELSE IU.[FullName] END
										,N.[ResponseNote]
										,[ResponseRequired]	= CASE WHEN N.[ResponseNote] IS NOT NULL THEN 0
																   ELSE N.[ResponseRequired] END
										,[ResponseDate] = DATEADD(HOUR,-6,N.[ResponseDate])
										,N.[ReadFlag]
								FROM	[config].[utbUserNotes] N
										LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = N.[UserID]
										LEFT JOIN [adm].[utbUsers] IU ON IU.[UserID] = N.[InsertUserID]
										LEFT JOIN [adm].[utbRoles] R ON R.[RoleID] = IU.[RoleID]
								WHERE	N.[NoteID] = @NoteID
					END
				ELSE
					BEGIN
						IF (@HistoryFlag = 1)
							BEGIN
								SELECT	N.[NoteID]
										,N.[UserID]
										,N.[RequestNote]
										,[NoteDate]		= DATEADD(HOUR,-6,N.[InsertDate])
										,[InsertedBy]	= CASE WHEN R.[RoleName] LIKE '%Admin%' THEN 'Grupo Administrador' 
															   ELSE IU.[FullName] END
										,N.[ResponseNote]
										,[ResponseRequired]	= CASE WHEN N.[ResponseNote] IS NOT NULL THEN 0
																   ELSE N.[ResponseRequired] END
										,[ResponseDate] = DATEADD(HOUR,-6,N.[ResponseDate])
										,N.[ReadFlag]
								FROM	[config].[utbUserNotes] N
										LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = N.[UserID]
										LEFT JOIN [adm].[utbUsers] IU ON IU.[UserID] = N.[InsertUserID]
										LEFT JOIN [adm].[utbRoles] R ON R.[RoleID] = IU.[RoleID]
								WHERE	U.[UserName] = @UserName
										AND N.[ActiveFlag] = 1
								ORDER BY N.[ReadFlag], N.[InsertDate] DESC
							END
						ELSE
							BEGIN
								SELECT	TOP 1 
										N.[NoteID]
										,N.[UserID]
										,N.[RequestNote]
										,[NoteDate]		= DATEADD(HOUR,-6,N.[InsertDate])
										,[InsertedBy]	= CASE WHEN R.[RoleName] LIKE '%Admin%' THEN 'Grupo Administrador' 
															   ELSE IU.[FullName] END
										,N.[ResponseNote]
										,[ResponseRequired]	= CASE WHEN N.[ResponseNote] IS NOT NULL THEN 0
																   ELSE N.[ResponseRequired] END
										,[ResponseDate] = CASE WHEN N.[ResponseDate] IS NOT NULL THEN DATEADD(HOUR,-6,N.[ResponseDate]) 
															   ELSE DATEADD(HOUR,-6,N.[InsertDate]) END
										,N.[ReadFlag]
								FROM	[config].[utbUserNotes] N
										LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = N.[UserID]
										LEFT JOIN [adm].[utbUsers] IU ON IU.[UserID] = N.[InsertUserID]
										LEFT JOIN [adm].[utbRoles] R ON R.[RoleID] = IU.[RoleID]
								WHERE	U.[UserName] = @UserName
										AND N.[ActiveFlag] = 1
										AND N.[ReadFlag] = 0
								ORDER BY R.[RoleID], N.[InsertDate] DESC
							END
					END
			-- =======================================================

        END TRY
        BEGIN CATCH

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END
    SET NOCOUNT OFF
