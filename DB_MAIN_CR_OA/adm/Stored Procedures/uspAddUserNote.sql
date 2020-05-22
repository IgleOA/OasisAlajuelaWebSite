-- ======================================================================
-- Name: [adm].[uspAddUserNote]
-- Desc: Se utiliza para agregar una nota al usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddUserNote]
	@InsertUser			VARCHAR(50),
	@UserID				INT,
	@RequestNote		VARCHAR(MAX),
	@ResponseRequired	BIT
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
				DECLARE @InsertUserID INT

				SELECT	@InsertUserID = [UserID]
				FROM	[adm].[utbUsers]
				WHERE	[UserName] = @InsertUser

				INSERT INTO [config].[utbUserNotes] ([UserID],[RequestNote],[ResponseRequired],[InsertUserID],[LastModifyUser])
				VALUES (@UserID,@RequestNote,@ResponseRequired,@InsertUserID,@InsertUser)
				
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