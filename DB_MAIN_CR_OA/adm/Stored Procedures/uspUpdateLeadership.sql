-- ======================================================================
-- Name: [adm].[uspUpdateLeadership]
-- Desc: Se utiliza para actualizar la información de un lider
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/28/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateLeadership]
	@InsertUser		VARCHAR(50),
	@LeaderID		INT,
	@FullName		VARCHAR(100) = NULL,
	@Description	VARCHAR(MAX) = NULL,
	@Image			VARCHAR(500) = NULL,
	@Order			INT = NULL,
	@ActionLink		VARCHAR(50) = NULL,
	@UpdateType		VARCHAR(10) = NULL
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
				IF(@UpdateType = 'DISABLE')
					BEGIN
						UPDATE	[config].[utbLeadership]
						SET		[ActiveFlag] =	0
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[LeaderID] = @LeaderID
					END
				ELSE
					BEGIN
						UPDATE	[config].[utbLeadership]
						SET		[FullName] = @FullName
								,[Description] = @Description
								,[ImagePath] = ISNULL(@Image,[ImagePath])
								,[ActionLink] = @ActionLink
								,[Order] = ISNULL(@Order,[Order])
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[LeaderID] = @LeaderID
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