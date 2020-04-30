-- ======================================================================
-- Name: [book].[uspReadWorships]
-- Desc: Retorna La informacion basica de un servicio para la reservación
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [book].[uspReadWorships]
	@WorshipID	INT 
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================								
				SELECT	W.[WorshipID]
						,W.[Capacity]
						,W.[ScheduledDate]
						,A.[Available]
						,[Unavailable] = A.[Total] - A.[Available]
						,B.[Booked]
				FROM	[book].[utbWorships] W
						OUTER APPLY (SELECT [Available] = CASE WHEN W.[Capacity] = 25 THEN SUM(IIF(S.[25Pct]=1,1,0))
																WHEN W.[Capacity] = 50 THEN SUM(IIF(S.[50Pct]=1,1,0))
																ELSE COUNT(S.[SeatID]) END
											,[Total] = COUNT(S.[SeatID])
										FROM	[book].[utbAuditoriumSeats] S) A
						OUTER APPLY (SELECT [Booked] = COUNT(R.[ReservationID])
										FROM	[book].[utbReservations] R
										WHERE	R.[WorshipID] = W.[WorshipID]
											AND R.[ActiveFlag] = 1) B
				WHERE	W.[WorshipID] = ISNULL(@WorshipID,W.[WorshipID])
						AND W.[ActiveFlag] = 1
				ORDER BY W.ScheduledDate					
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