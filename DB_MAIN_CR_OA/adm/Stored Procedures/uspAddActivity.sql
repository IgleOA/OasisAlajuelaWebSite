-- ======================================================================
-- Name: [adm].[uspAddActivity]
-- Desc: Se utiliza para agregar una de un usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddActivity]
	@User			VARCHAR(50),	
	@Controller		VARCHAR(50),
	@Action			VARCHAR(50),
	@ActivityDate	DATETIME
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
				DECLARE @UserID INT
				SELECT	@UserID = [UserID]
				FROM	[adm].[utbUsers]
				WHERE	[UserName] = @User

				INSERT INTO [adm].[utbUsersActivities] ([UserID],[Controller],[Action],[ActivityDate])
				VALUES (@UserID, @Controller, @Action, @ActivityDate)

				UPDATE	[adm].[utbUsers]
				SET		[LastActivityDate] = @ActivityDate
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
