-- ======================================================================
-- Name: [book].[uspReadMainInfoReservationsbyUser]
-- Desc: Retorna las reservaciones hechas por un determinado usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [book].[uspReadMainInfoReservationsbyUser]
	@UserID		INT = NULL,
	@EventID	INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================	
				IF(@UserID IS NOT NULL)
					BEGIN
						SELECT	DISTINCT
								R.[GUID]
								,R.[EventID]
								,W.[Title]
								,W.[ScheduledDate]
								,[ActiveFlag]		= CASE WHEN W.[ScheduledDate] < DATEADD(HOUR,-6,GETDATE()) THEN 0 ELSE 1 END
								,[ReservationDate]	= CONVERT(DATE,DATEADD(HOUR,-6,R.[InsertDate]))
						FROM	[book].[utbReservations] R
								LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = R.[BookedBy]
								LEFT JOIN [config].[utbUpcomingEvents] W ON W.[EventID] = R.[EventID] AND W.[ActiveFlag] = 1
						WHERE	R.[BookedBy] = @UserID
								AND R.[EventID] = ISNULL(@EventID, R.[EventID])
								AND R.[ActiveFlag] = 1		
								AND (CASE WHEN W.[ScheduledDate] < DATEADD(HOUR,-6,GETDATE()) THEN 0 ELSE 1 END) = 1
						UNION
						SELECT	DISTINCT TOP 3
								R.[GUID]
								,R.[EventID]
								,W.[Title]
								,W.[ScheduledDate]
								,[ActiveFlag]		= CASE WHEN W.[ScheduledDate] < DATEADD(HOUR,-6,GETDATE()) THEN 0 ELSE 1 END
								,[ReservationDate]	= CONVERT(DATE,DATEADD(HOUR,-6,R.[InsertDate]))
						FROM	[book].[utbReservations] R
								LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = R.[BookedBy]
								LEFT JOIN [config].[utbUpcomingEvents] W ON W.[EventID] = R.[EventID] AND W.[ActiveFlag] = 1
						WHERE	R.[BookedBy] = @UserID
								AND R.[EventID] = ISNULL(@EventID, R.[EventID])
								AND R.[ActiveFlag] = 1			
						ORDER BY W.[ScheduledDate], R.[EventID], [ReservationDate]
					END
				ELSE
					BEGIN
						SELECT	DISTINCT
								R.[GUID]
								,R.[EventID]
								,W.[Title]
								,W.[ScheduledDate]
								,[ActiveFlag]		= CASE WHEN W.[ScheduledDate] < DATEADD(HOUR,-6,GETDATE()) THEN 0 ELSE 1 END
								,[ReservationDate]	= CONVERT(DATE,DATEADD(HOUR,-6,R.[InsertDate]))
								,R.[BookedBy]
								,[BookedByName]		= U.[FullName]
								,R.[BookedFor]
						FROM	[book].[utbReservations] R
								LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = R.[BookedBy]
								LEFT JOIN [config].[utbUpcomingEvents] W ON W.[EventID] = R.[EventID] AND W.[ActiveFlag] = 1
						WHERE	R.[ActiveFlag] = 1		
								AND (CASE WHEN W.[ScheduledDate] < DATEADD(HOUR,-6,GETDATE()) THEN 0 ELSE 1 END) = 1
					END
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