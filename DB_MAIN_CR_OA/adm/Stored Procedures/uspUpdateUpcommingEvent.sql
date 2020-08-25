-- ======================================================================
-- Name: [adm].[uspUpdateUpcommingEvent]
-- Desc: Se utiliza para actualizar la información de un evento
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/28/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateUpcommingEvent]
	@InsertUser			VARCHAR(50),
	@UpdateType			VARCHAR(10),
	@EventID			INT,
	@Title				VARCHAR(50) = NULL,
	@MinisterID			INT = NULL,
	@Description		VARCHAR(MAX) = NULL,
	@ScheduleDate		DATETIME = NULL,
	@ReservationFlag	BIT = NULL,
	@Capacity			INT = NULL,
	@SocialDistance		INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        SET XACT_ABORT ON
                           
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT
            DECLARE @lLocalTran BIT = 0
                               
            IF @@TRANCOUNT = 0 
                BEGIN
                    BEGIN TRANSACTION
                    SET @lLocalTran = 1
                END

            -- =======================================================
				IF(@UpdateType = 'DISABLE')
					BEGIN						
						UPDATE	[config].[utbUpcomingEvents]
						SET		[ActiveFlag] =	0
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[EventID] = @EventID
						
					END
				ELSE
					BEGIN
						UPDATE	[config].[utbUpcomingEvents]
						SET		[Title]	=	@Title
                                ,[ActiveFlag] = 1
								,[MinisterID] = @MinisterID
								,[Description] = @Description
								,[ScheduledDate] = @ScheduleDate
								,[ReservationFlag] = @ReservationFlag
								,[Capacity] = @Capacity
								,[SocialDistance] =	@SocialDistance
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[EventID] = @EventID
					END
			-- =======================================================

        IF ( @@trancount > 0
                 AND @lLocalTran = 1
               ) 
                BEGIN
                    COMMIT TRANSACTION
                END
        END TRY
        BEGIN CATCH
            IF ( @@trancount > 0
                 AND XACT_STATE() <> 0
               ) 
                BEGIN
                    ROLLBACK TRANSACTION
                END

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END

    SET NOCOUNT OFF
    SET XACT_ABORT OFF