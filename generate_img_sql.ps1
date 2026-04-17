$ErrorActionPreference = 'Stop'
$todoPath = 'C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\TODO.md'
$sqlPath = 'C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\SQL\Seed\Taurus_update_product_images.sql'

$content = Get-Content $todoPath -Encoding UTF8 | Select-Object -Skip 3
$dict = @{}

foreach ($line in $content) {
    if ([string]::IsNullOrWhiteSpace($line)) { continue }
    $parts = $line -split "`t"
    if ($parts.Length -lt 2) { continue }
    $name = $parts[0].Trim().Replace("'", "''")
    $url = $parts[1].Trim().Replace("'", "''")
    # PowerShell hash tables are case-insensitive by default.
    $dict[$name] = "('$name', '$url')"
}

$valuesStr = $dict.Values -join ",`n            "

$sql = @"
-- =====================================================================
-- Taurus Bike Shop - Product Image Update Script
-- =====================================================================

MERGE INTO [dbo].[ProductImage] AS target
USING (
    -- Get Product IDs by name
    SELECT p.ProductId, updates.ImageUrl
    FROM [dbo].[Product] p
    JOIN (
        VALUES
            $valuesStr
    ) AS updates(ProductName, ImageUrl)
    ON p.Name = updates.ProductName
) AS source
-- Match ONLY the primary image to avoid updating multiple non-primary images and breaking unique constraint
ON (target.ProductId = source.ProductId AND target.IsPrimary = 1)
WHEN MATCHED THEN
    UPDATE SET 
        target.ImageUrl = source.ImageUrl,
        target.StorageBucket = 'taurus-bikeshop-assets',
        target.StoragePath = source.ImageUrl,
        target.ImageType = 'Full'
WHEN NOT MATCHED BY TARGET THEN
    INSERT (ProductId, StorageBucket, StoragePath, ImageUrl, ImageType, IsPrimary, DisplayOrder, CreatedAt)
    VALUES (source.ProductId, 'taurus-bikeshop-assets', source.ImageUrl, source.ImageUrl, 'Full', 1, 0, GETUTCDATE());
"@

Set-Content -Path $sqlPath -Value $sql -Encoding UTF8
Write-Output "Successfully generated $($sqlPath) with $($dict.Count) unique mappings."
