-- ======================================================================
-- Name: [adm].[uspAddResource]
-- Desc: Permite ingresar nuevos recursos.
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================
CREATE PROCEDURE [adm].[uspAddResource]
	@InsertUser		VARCHAR(50),	
	@ResourceTypeID	INT,
	@Description	VARCHAR(MAX),
	@FileType		VARCHAR(100),
	@FileData		VARBINARY(MAX)	= NULL,
	@FileExt		VARCHAR(10)		= NULL,
	@FileName		VARCHAR(500)	= NULL,
	@FileURL		VARCHAR(500)	= NULL
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
				INSERT INTO [config].[utbResources] ([ResourceTypeID],[FileType],[FileData],[FileExt],[FileName],[FileURL],[Description],[InsertUser],[LastModifyUser])
				VALUES (@ResourceTypeID,@FileType,@FileData,REPLACE(@FileExt,'.',''),@FileName,@FileURL,@Description ,@InsertUser,@InsertUser)

				SELECT [ResourceID] = SCOPE_IDENTITY()
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