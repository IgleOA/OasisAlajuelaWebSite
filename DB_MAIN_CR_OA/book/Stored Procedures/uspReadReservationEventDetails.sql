﻿-- ======================================================================
-- Name: [book].[uspReadReservationEventDetails]
-- Desc: Retorna La informacion basica de un servicio para la reservación
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [book].[uspReadReservationEventDetails]
	@EventID	INT,
	@UserID		INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================		
				DECLARE @AdminRole INT

				SELECT	@AdminRole = R.[RoleID]
				FROM	[adm].[utbUsers] U
						LEFT JOIN [adm].[utbRoles] R ON R.[RoleID] = U.[RoleID] AND R.[ActiveFlag] = 1
				WHERE	U.[UserID] = @UserID
						AND R.[RoleName] LIKE 'Admin%'

				SELECT	W.[EventID]
						,W.[Capacity]
						,W.[ScheduledDate]
						,[Available]	= A.[Available] - B.[Booked]
						,[Unavailable]	= A.[Total] - A.[Available]
						,B.[Booked]
						,[MaxToReserve]	= CASE WHEN @AdminRole IS NOT NULL THEN A.[Available] 
											   WHEN (30 - BBU.[Booked]) >= 10 THEN 10
											   ELSE 30 - BBU.[Booked] END

				FROM	[config].[utbUpcomingEvents] W
						OUTER APPLY (SELECT [Available] = CASE WHEN W.[Capacity] > 0 THEN W.[Capacity]
															   ELSE COUNT(S.[SeatID]) END
											,[Total] = COUNT(S.[SeatID])
									 FROM	[book].[utbAuditoriumSeats] S) A
						
						OUTER APPLY (SELECT [Booked] = COUNT(R.[ReservationID])
									 FROM	[book].[utbReservations] R
									 WHERE	R.[EventID] = W.[EventID]
											AND R.[ActiveFlag] = 1) B

						OUTER APPLY (SELECT [Booked] = COUNT(R.[ReservationID])
									 FROM	[book].[utbReservations] R
									 WHERE	R.[EventID] = W.[EventID]
											AND R.[BookedBy] = @UserID
											AND R.[ActiveFlag] = 1) BBU

				WHERE	W.[EventID] = @EventID
						AND W.[ActiveFlag] = 1
						AND W.[ReservationFlag] = 1
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