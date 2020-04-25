-- ======================================================================
-- Name: [adm].[uspAddResourceGroup]
-- Desc: Se utiliza para agregar recursos a grupos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddResourceGroup]
	@InsertUser		VARCHAR(50),	
	@GroupID		INT,
	@ResourceTypeID	INT
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
				DECLARE @ResourceGroupID INT

				SELECT	@ResourceGroupID = RG.[ResourceGroupID]
				FROM	[config].[utbResourcesGroups] RG											
				WHERE	RG.[ResourceTypeID] = @ResourceTypeID
						AND RG.[GroupID] = @GroupID

				IF(@ResourceGroupID IS NOT NULL)
					BEGIN
						UPDATE	[config].[utbResourcesGroups]
						SET		[ActiveFlag] = 1
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[ResourceGroupID] = @ResourceGroupID
					END
				ELSE
					BEGIN
						INSERT INTO [config].[utbResourcesGroups] ([ResourceTypeID],[GroupID],[InsertUser],[LastModifyUser])
						VALUES (@ResourceTypeID,@GroupID,@InsertUser,@InsertUser)
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
