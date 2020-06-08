-- ======================================================================
-- Name: [adm].[uspRemoveSubscriber]
-- Desc: Se utiliza para remover un subcriptor
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/02/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspRemoveSubscriber]
	@Email	VARCHAR(50)
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
				DECLARE	@UserID			INT
				DECLARE @StartValues	XML
				DECLARE @EndValues		XML
				CREATE TABLE #DBCC (EventType varchar(50), Parameters varchar(50), EventInfo nvarchar(max))

				SELECT	@UserID = [UserID]
				FROM	[adm].[utbUsers]
				WHERE	[Email] = @Email
				
				SET @StartValues	= (SELECT	[UserID],[RoleID],[FullName],[UserName],[Email],[Subscriber],[ActiveFlag],[LastActivityDate],[CreationDate],[CreationUser],[LastModifyDate],[LastModifyUser] 
										FROM	[adm].[utbUsers] 
										WHERE	[UserID] = @UserID for xml AUTO, ELEMENTS XSINIL)					
						
				UPDATE	[adm].[utbUsers]
				SET		[Subscriber] = 0
						,[LastModifyDate] = GETDATE()
						,[LastModifyUser] = [UserName]
				WHERE	[UserID] = @UserID

				SET @EndValues	= (SELECT	[UserID],[RoleID],[FullName],[UserName],[Email],[Subscriber],[ActiveFlag],[LastActivityDate],[CreationDate],[CreationUser],[LastModifyDate],[LastModifyUser] 
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