-- ======================================================================
-- Name: [book].[uspReadAuditoriumRows]
-- Desc: Retorna los detalles del Layout del Auditorio
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [book].[uspReadAuditoriumRows]
	@BlockID VARCHAR(10)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[RowID]
						,[BlockID]
						,[Label] = CONVERT(INT,RIGHT([RowID],1))
						,[Seats]		
				FROM	[book].[utbAuditoriumRows]
				WHERE	[BlockID] = @BlockID
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