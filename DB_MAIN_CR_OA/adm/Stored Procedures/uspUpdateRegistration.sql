-- ======================================================================
-- Name: [adm].[uspUpdateRegistration]
-- Desc: Se utiliza para actualizar los datos de una matriculado
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/28/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateRegistration]
	@RegisterID		INT,
	@FullName		VARCHAR(100) = NULL,
	@PhoneNumber	VARCHAR(100) = NULL,
	@ActiveFlag		BIT = NULL
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
				IF(@ActiveFlag IS NOT NULL)
					BEGIN
						UPDATE	[config].[utbRegistrations] 
						SET		[ActiveFlag] = @ActiveFlag
						WHERE	[RegisterID] = @RegisterID	
					END
				ELSE
					BEGIN
						UPDATE	[config].[utbRegistrations] 
						SET		[FullName]		= ISNULL(@FullName,[FullName])
								,[PhoneNumber]	= ISNULL(@PhoneNumber,[PhoneNumber])
						WHERE	[RegisterID]	= @RegisterID				
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