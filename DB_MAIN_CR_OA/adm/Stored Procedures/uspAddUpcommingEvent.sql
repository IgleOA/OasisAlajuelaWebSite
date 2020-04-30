-- ======================================================================
-- Name: [adm].[uspAddUpcommingEvent]
-- Desc: Se utiliza para agregar un nuevo evento
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/28/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddUpcommingEvent]
	@InsertUser		VARCHAR(50),
	@Title			VARCHAR(50),
	@MinisterID		INT,
	@Description	VARCHAR(MAX) = NULL,
	@ScheduleDate	DATETIME,
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
				IF (@Capacity IS NULL)
					BEGIN
						INSERT INTO [config].[utbUpcomingEvents]([Title],[MinisterID],[Description],[ScheduledDate],[InsertUser],[LastModifyUser])
						VALUES (@Title, @MinisterID, @Description, @ScheduleDate, @InsertUser, @InsertUser)
					END
				ELSE
					BEGIN
						INSERT INTO [config].[utbUpcomingEvents]([Title],[MinisterID],[Description],[ScheduledDate],[InsertUser],[LastModifyUser])
						VALUES (@Title, @MinisterID, @Description, @ScheduleDate, @InsertUser, @InsertUser)

						INSERT INTO [book].[utbWorships] ([ScheduledDate],[Capacity],[InsertUser],[LastModifyUser])
						VALUES (@ScheduleDate, @Capacity, @InsertUser, @InsertUser)
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