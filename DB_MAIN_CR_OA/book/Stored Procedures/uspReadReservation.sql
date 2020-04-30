-- ======================================================================
-- Name: [book].[uspReadReservation]
-- Desc: Retorna los detalles de una reserva
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [book].[uspReadReservation]
	@GUID VARCHAR(MAX)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	R.[ReservationID]
						,R.[GUID]
						,R.[WorshipID]
						,R.[SeatID]
						,R.[BookedBy]
						,[BookedByName] = U.[FullName]
						,R.[BookedFor]
				FROM	[book].[utbReservations] R
						LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = R.[BookedBy]
				WHERE	R.[GUID] = @GUID
						AND R.[ActiveFlag] = 1
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