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
	@InsertUser		VARCHAR(50),
	@EventID		INT,
	@Title			VARCHAR(50),
	@MinisterID		INT,
	@Description	VARCHAR(MAX) = NULL,
	@ScheduleDate	DATETIME,
	@UpdateType		VARCHAR(10) = NULL,
	@Capacity		INT = NULL
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
				DECLARE @WorshipID INT

				SELECT	@WorshipID = [WorshipID]
				FROM	[book].[utbWorships]
				WHERE	[ActiveFlag] = 1
						AND [ScheduledDate] =  (SELECT [ScheduledDate] 
												FROM [config].[utbUpcomingEvents]
												WHERE	[EventID] = @EventID)

				IF(@UpdateType = 'DISABLE')
					BEGIN						
						UPDATE	[config].[utbUpcomingEvents]
						SET		[ActiveFlag] =	0
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[EventID] = @EventID

						UPDATE	[book].[utbWorships]
						SET		[ActiveFlag] =	0
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[WorshipID] = @WorshipID
						
					END
				ELSE
					BEGIN
						IF(@WorshipID >= 1)
							BEGIN
								UPDATE	[config].[utbUpcomingEvents]
								SET		[Title]	=	@Title
										,[MinisterID] = @MinisterID
										,[Description] = @Description
										,[ScheduledDate] = @ScheduleDate
										,[LastModifyDate] = GETDATE()
										,[LastModifyUser] = @InsertUser
								WHERE	[EventID] = @EventID

								IF(@Capacity > 0)
									BEGIN
										UPDATE	[book].[utbWorships]
										SET		[ScheduledDate] = @ScheduleDate
												,[Capacity] = @Capacity
												,[LastModifyDate] = GETDATE()
												,[LastModifyUser] = @InsertUser
										WHERE	[WorshipID] = @WorshipID
									END
								ELSE
									BEGIN
										UPDATE	[book].[utbWorships]
										SET		[ActiveFlag] =	0
												,[LastModifyDate] = GETDATE()
												,[LastModifyUser] = @InsertUser
										WHERE	[WorshipID] = @WorshipID
									END
							END
						ELSE
							BEGIN
								UPDATE	[config].[utbUpcomingEvents]
								SET		[Title]	=	@Title
										,[MinisterID] = @MinisterID
										,[Description] = @Description
										,[ScheduledDate] = @ScheduleDate
										,[LastModifyDate] = GETDATE()
										,[LastModifyUser] = @InsertUser
								WHERE	[EventID] = @EventID

								IF(@Capacity > 0)
									BEGIN
										INSERT INTO [book].[utbWorships] ([ScheduledDate],[Capacity],[InsertUser],[LastModifyUser])
										VALUES (@ScheduleDate, @Capacity, @InsertUser, @InsertUser)
									END
							END
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