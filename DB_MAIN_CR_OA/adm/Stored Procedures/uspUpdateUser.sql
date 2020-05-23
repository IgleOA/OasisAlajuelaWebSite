-- ======================================================================
-- Name: [adm].[uspUpdateUser]
-- Desc: Se utiliza para actualizar la información de un usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/02/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateUser]
	@InsertUser		VARCHAR(50),
	@UserID			INT,
	@ActionType		VARCHAR(10),
	@FullName		VARCHAR(50) = NULL,
	@UserName		VARCHAR(50) = NULL,
	@Email			VARCHAR(50) = NULL,
	@ActiveFlag		BIT = NULL,
	@RoleID			INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        SET XACT_ABORT ON
                           
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT
            DECLARE @lLocalTran BIT = 0
                               
            IF @@TRANCOUNT = 0 
                BEGIN
                    BEGIN TRANSACTION
                    SET @lLocalTran = 1
                END

            -- =======================================================
				DECLARE @Status			BIT
				DECLARE @StartValues	XML
				DECLARE @EndValues		XML
				CREATE TABLE #DBCC (EventType varchar(50), Parameters varchar(50), EventInfo nvarchar(max))

				IF(@ActionType = 'CHGST')
					BEGIN
						SET @Status			= (SELECT [ActiveFlag] FROM [adm].[utbUsers] WHERE [UserID] = @UserID) 

						SET @StartValues	= (SELECT	[UserID],[RoleID],[FullName],[UserName],[Email],[ActiveFlag],[LastActivityDate],[CreationDate],[CreationUser],[LastModifyDate],[LastModifyUser] 
											   FROM		[adm].[utbUsers] 
											   WHERE	[UserID] = @UserID for xml AUTO, ELEMENTS XSINIL)					
						
						UPDATE	[adm].[utbUsers]
						SET		[ActiveFlag]	= CASE WHEN @Status = 1 THEN 0 ELSE 1 END
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[UserID]		= @UserID

						SET @EndValues			= (SELECT	[UserID],[RoleID],[FullName],[UserName],[Email],[ActiveFlag],[LastActivityDate],[CreationDate],[CreationUser],[LastModifyDate],[LastModifyUser] 
												   FROM		[adm].[utbUsers] 
												   WHERE	[UserID] = @UserID for xml AUTO, ELEMENTS XSINIL)
						
						INSERT INTO #DBCC
						EXEC ('DBCC INPUTBUFFER(@@SPID)')

						INSERT INTO [adm].[utbLogActivities] ([ActivityType],[TargetTable],[SQLStatement],[StartValues],[EndValues],[User],[LogActivityDate])
						SELECT	'UPDATE'
								,'[adm].[utbUsers]'
								,(SELECT EventInfo FROM #DBCC)
								,@StartValues
								,@EndValues
								,[LastModifyUser]
								,GETDATE()
						FROM	[adm].[utbUsers]
						WHERE	[UserID] = @UserID

					END
				ELSE
					BEGIN
						SET @Status			= (SELECT [ActiveFlag] FROM [adm].[utbUsers] WHERE [UserID] = @UserID) 
						SET @StartValues	= (SELECT	[UserID],[RoleID],[FullName],[UserName],[Email],[ActiveFlag],[LastActivityDate],[CreationDate],[CreationUser],[LastModifyDate],[LastModifyUser] 
											   FROM		[adm].[utbUsers]
											   WHERE	[UserID] = @UserID for xml AUTO, ELEMENTS XSINIL)	

						UPDATE	[adm].[utbUsers] 
						SET		[RoleID]		= @RoleID
								,[FullName]		= @FullName
								,[UserName]		= @UserName
								,[Email]		= @Email
								,[ActiveFlag]	= @ActiveFlag
								,[LastModifyUser] = @InsertUser
								,[LastModifyDate] = GETDATE()
						WHERE	[UserID]	= @UserID

						SET		@EndValues	= (SELECT	[UserID],[RoleID],[FullName],[UserName],[Email],[ActiveFlag],[LastActivityDate],[CreationDate],[CreationUser],[LastModifyDate],[LastModifyUser] 
											   FROM		[adm].[utbUsers] 
											   WHERE	[UserID] = @UserID for xml AUTO, ELEMENTS XSINIL)
						
						INSERT INTO #DBCC
						EXEC ('DBCC INPUTBUFFER(@@SPID)')

						INSERT INTO [adm].[utbLogActivities] ([ActivityType],[TargetTable],[SQLStatement],[StartValues],[EndValues],[User],[LogActivityDate])
						SELECT	'UPDATE'
								,'[adm].[utbUsers]'
								,(SELECT EventInfo FROM #DBCC)
								,@StartValues
								,@EndValues
								,[LastModifyUser]
								,GETDATE()
						FROM	[adm].[utbUsers]
						WHERE	[UserID] = @UserID

					END 
			-- =======================================================

        IF ( @@trancount > 0
                 AND @lLocalTran = 1
               ) 
                BEGIN
                    COMMIT TRANSACTION
                END
        END TRY
        BEGIN CATCH
            IF ( @@trancount > 0
                 AND XACT_STATE() <> 0
               ) 
                BEGIN
                    ROLLBACK TRANSACTION
                END

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END

    SET NOCOUNT OFF
    SET XACT_ABORT OFF