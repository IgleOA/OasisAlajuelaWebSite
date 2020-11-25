-- ======================================================================
-- Name: [adm].[uspUpdateHomePage]
-- Desc: Se utiliza para la actualizar una seccion del HomePage
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateHomePage]
	@InsertUser		VARCHAR(50),
	@SectionID		INT,
	@Title			VARCHAR(100) = NULL,
	@Description    VARCHAR(MAX) = NULL,
    @RouterLink     VARCHAR(100) = NULL,
    @Image          VARCHAR(500) = NULL,
    @Order          INT = NULL,
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
				UPDATE	[config].[utbHomePage] 
				SET		[Title]				= ISNULL(@Title,[Title])
						,[Description]		= ISNULL(@Description,[Description])
                        ,[RouterLink]       = ISNULL(@RouterLink,[RouterLink])
                        ,[Image]            = ISNULL(@Image,[Image])
                        ,[Order]            = ISNULL(@Order,[Order])
						,[ActiveFlag]		= ISNULL(@ActiveFlag,[ActiveFlag])
						,[LastModifyUser]	= @InsertUser
						,[LastModifyDate]	= GETDATE()
				WHERE	[SectionID] = @SectionID
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