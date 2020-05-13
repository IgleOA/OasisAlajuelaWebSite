-- ======================================================================
-- Name: [book].[uspReadReservationsByUser]
-- Desc: Retorna las reservaciones hechas por un determinado usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [book].[uspReadReservationsByUser]
	@UserID		INT,
	@EventID	INT = NULL
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
						,R.[SeatID]
						,R.[BookedBy]
						,[BookedByName]		= U.[FullName]
						,R.[BookedFor]
						,[ActiveFlag]		= CASE WHEN W.[ScheduledDate] < DATEADD(HOUR,-6,GETDATE()) THEN 0 ELSE 1 END
						,[ReservationDate]	= DATEADD(HOUR,-6,R.[InsertDate])
				FROM	[book].[utbReservations] R
						LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = R.[BookedBy]
						INNER JOIN [config].[utbUpcomingEvents] W ON W.[EventID] = R.[EventID] AND W.[ActiveFlag] = 1
				WHERE	R.[BookedBy] = @UserID
						AND R.[EventID] = ISNULL(@EventID, R.[EventID])
						AND R.[ActiveFlag] = 1			
				ORDER BY W.[ScheduledDate], R.[EventID], R.[InsertDate], R.[BookedFor]
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