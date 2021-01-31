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
	@GUID VARCHAR(MAX) = NULL,
	@EventID INT = NULL,
	@ReservationID INT = NULL
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
						,R.[EventID]
						,W.[Title]
						,W.[ScheduledDate]
						,R.[BookedBy]
						,[BookedByName]		= U.[FullName]
						,R.[FirstName]
						,R.[LastName]
						,R.[IdentityID]
						,[ReservationDate]	= DATEADD(HOUR,-6,R.[InsertDate])
				FROM	[book].[utbReservations] R
						LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = R.[BookedBy]
						LEFT JOIN [config].[utbUpcomingEvents]  W ON W.[EventID] = R.[EventID] AND W.[ActiveFlag] = 1
				WHERE	R.[GUID] = ISNULL(@GUID,R.[GUID])
						AND R.[EventID] = ISNULL(@EventID,R.[EventID])
						AND R.[ReservationID] = ISNULL(@ReservationID,R.[ReservationID])
						AND R.[ActiveFlag] = 1
				ORDER BY R.[LastName], R.[FirstName]
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