-- ======================================================================
-- Name: [config].[uspReadPrayers]
-- Desc: Retorna las solicitudes de oración
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadPrayers]
	@PrayerID		INT = NULL,
	@HistoryFlag	BIT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				IF(@PrayerID IS NOT NULL)
					BEGIN
						SELECT	N.[PrayerID]
								,N.[Requester]
								,N.[Email]
								,N.[PhoneNumber]
								,[InsertDate]	= DATEADD(HOUR,-6,N.[InsertDate])
								,N.[Reason]
								,N.[IP]
								,N.[Country]
								,N.[Region]
								,N.[City]
						FROM	[config].[utbPayers] N
						WHERE	N.[PrayerID] = @PrayerID
					END
				ELSE
					BEGIN
						IF(@HistoryFlag = 1)
							BEGIN
								SELECT	N.[PrayerID]
										,N.[Requester]
										,N.[Email]
										,N.[PhoneNumber]
										,[InsertDate]	= DATEADD(HOUR,-6,N.[InsertDate])
										,N.[Reason]
										,N.[IP]
										,N.[Country]
										,N.[Region]
										,N.[City]
								FROM	[config].[utbPayers] N
								ORDER BY [InsertDate] DESC
							END
						ELSE -- Prayers of the last month
							BEGIN
								SELECT	N.[PrayerID]
										,N.[Requester]
										,N.[Email]
										,N.[PhoneNumber]
										,[InsertDate]	= DATEADD(HOUR,-6,N.[InsertDate])
										,N.[Reason]
										,N.[IP]
										,N.[Country]
										,N.[Region]
										,N.[City]
								FROM	[config].[utbPayers] N
								WHERE	DATEDIFF(MONTH,DATEADD(HOUR,-6,N.[InsertDate]),GETDATE()) = 0
								ORDER BY [InsertDate] DESC
							END
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
