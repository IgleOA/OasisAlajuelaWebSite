-- ======================================================================
-- Name: [book].[uspReadAuditoriumSeats]
-- Desc: Retorna los detalles del Layout del Auditorio
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [book].[uspReadAuditoriumSeats]
	@RowID		VARCHAR(10),
	@EventID	INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	ADS.[SeatID]
						,ADS.[RowID]
						,ADS.[Label]
						,[Reserved]		= [book].[ufnCheckSeatAvailability] (@EventID,ADS.[SeatID],WW.[SocialDistance])
						,[Booked]		= ISNULL(R.[BookedBy],0)
						,[BookedFor]	= R.[BookedFor]
				FROM	[book].[utbAuditoriumSeats] ADS
						LEFT JOIN [config].[utbUpcomingEvents] WW ON WW.[EventID] = @EventID
						LEFT JOIN [book].[utbReservations] R ON R.[SeatID] = ADS.[SeatID] AND R.[ActiveFlag] = 1 AND R.[EventID] = @EventID
						LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = R.[BookedBy]
				WHERE	ADS.[RowID] = @RowID
				ORDER BY ADS.[Label]
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