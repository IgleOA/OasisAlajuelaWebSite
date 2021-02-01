-- ======================================================================
-- Name: [book].[uspReadReservation]
-- Desc: Retorna un distinct de todas las personas que han hecho reservas
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 02/01/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [book].[uspReadReservers]
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	DISTINCT 
		                [IdentityID]
		                ,[FirstName]
		                ,[LastName]      
                FROM	[book].[utbReservations]
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