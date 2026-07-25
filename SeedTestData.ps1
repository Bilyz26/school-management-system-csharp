# ============================================================
# SeedTestData.ps1 - v2: Robust test data for SchoolData.db
# 15 subjects | 20 professors | 45 groups | 400 students
# ============================================================

$dbPath  = Join-Path $PSScriptRoot "prj_ForYou\bin\Release\SchoolData.db"
$dllPath = Join-Path $PSScriptRoot "prj_ForYou\bin\Release\System.Data.SQLite.dll"

Write-Host "Loading SQLite..." -ForegroundColor Cyan
[System.Reflection.Assembly]::LoadFrom($dllPath) | Out-Null

$connStr = "Data Source=$dbPath;Version=3;"
$conn    = New-Object System.Data.SQLite.SQLiteConnection($connStr)
$conn.Open()
Write-Host "Connected to $dbPath" -ForegroundColor Green

function Exec($sql, [hashtable]$p = @{}) {
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $sql
    foreach ($k in $p.Keys) { $cmd.Parameters.AddWithValue($k, $p[$k]) | Out-Null }
    try { $cmd.ExecuteNonQuery() | Out-Null } catch { }
    $cmd.Dispose()
}

# ─── 0. Clear old test data ───────────────────────────────
Write-Host "`nClearing old data..." -ForegroundColor Yellow
foreach ($t in @("pay","Raff","seance","grp","niveauMat","inscStd","prof","matier")) {
    Exec "DELETE FROM $t"
}
Exec "DELETE FROM emp WHERE username <> 'admin'"
Write-Host "Done." -ForegroundColor Green

# ─── 1. Academic years ───────────────────────────────────
foreach ($y in @(2024,2025,2026)) { Exec "INSERT OR IGNORE INTO Annee(annee) VALUES ($y)" }

# ─── 2. Staff ────────────────────────────────────────────
Write-Host "Inserting staff..." -ForegroundColor Cyan
@(
    @("Karim Benali",     "0661001001","Secrétaire", "kbenali",    "pass123"),
    @("Fatima Zohra",     "0661001002","Comptable",  "fzohra",     "pass123"),
    @("Hassan Moussaoui", "0661001003","Surveillant","hmoussaoui", "pass123")
) | ForEach-Object {
    Exec "INSERT OR IGNORE INTO emp(nomemp,tele,fonction,username,pw) VALUES(@a,@b,@c,@d,@e)" `
         @{"@a"=$_[0];"@b"=$_[1];"@c"=$_[2];"@d"=$_[3];"@e"=$_[4]}
}

# ─── 3. Subjects (15) ────────────────────────────────────
Write-Host "Inserting 15 subjects..." -ForegroundColor Cyan
$matieres = [ordered]@{
    "MAT01"="Mathématiques";  "MAT02"="Physique-Chimie"; "MAT03"="Langue Française";
    "MAT04"="Langue Arabe";   "MAT05"="Anglais";          "MAT06"="Informatique";
    "MAT07"="Histoire-Géo";   "MAT08"="SVT";              "MAT09"="Philosophie";
    "MAT10"="Économie";       "MAT11"="Comptabilité";     "MAT12"="Espagnol";
    "MAT13"="Éducation Islam";"MAT14"="Arts Plastiques";  "MAT15"="Éducation Physique"
}
foreach ($id in $matieres.Keys) {
    Exec "INSERT OR IGNORE INTO matier(idmat,nomMat) VALUES(@i,@n)" `
         @{"@i"=$id; "@n"=$matieres[$id]}
}

# ─── 4. NiveauMat (3 levels × 15 = 45) ──────────────────
Write-Host "Inserting 45 niveau-matière..." -ForegroundColor Cyan
$niveaux = [ordered]@{ "TC"="Tronc Commun"; "1BAC"="1ère Bac"; "2BAC"="2ème Bac" }
foreach ($mid in $matieres.Keys) {
    foreach ($nk in $niveaux.Keys) {
        Exec "INSERT OR IGNORE INTO niveauMat(codeNiv,[#idmat],nomMat) VALUES(@c,@i,@n)" `
             @{"@c"="${mid}_${nk}"; "@i"=$mid; "@n"=($matieres[$mid]+" - "+$niveaux[$nk])}
    }
}

# ─── 5. Professors (20) ──────────────────────────────────
Write-Host "Inserting 20 professors..." -ForegroundColor Cyan
$profList = @(
    @("Ahmed Bensalem",    "0670111001","MAT01"), @("Nadia Chraibi",    "0670111002","MAT01"),
    @("Youssef Alami",     "0670111003","MAT02"), @("Samira Tazi",      "0670111004","MAT02"),
    @("Rachid Kadiri",     "0670111005","MAT03"), @("Houda Mansouri",   "0670111006","MAT03"),
    @("Omar El Fassi",     "0670111007","MAT04"), @("Zineb Berrada",    "0670111008","MAT04"),
    @("Khalid Lahlou",     "0670111009","MAT05"), @("Imane Bakkali",    "0670111010","MAT05"),
    @("Tariq Bensouda",    "0670111011","MAT06"), @("Meriem Filali",    "0670111012","MAT06"),
    @("Driss Hamdouni",    "0670111013","MAT07"), @("Latifa Sekkat",    "0670111014","MAT08"),
    @("Hamid Bouazza",     "0670111015","MAT09"), @("Soumia Cherkaoui", "0670111016","MAT10"),
    @("Abdelkader Znati",  "0670111017","MAT11"), @("Fatine Rhazali",   "0670111018","MAT12"),
    @("Mourad Belkadi",    "0670111019","MAT13"), @("Ilham Benkirane",  "0670111020","MAT14")
)
foreach ($p in $profList) {
    Exec "INSERT OR IGNORE INTO prof(nomprof,teleprof,[#idmat]) VALUES(@n,@t,@i)" `
         @{"@n"=$p[0]; "@t"=$p[1]; "@i"=$p[2]}
}

# Build subject -> professors lookup
$matProfs = @{}
foreach ($p in $profList) {
    if (-not $matProfs.ContainsKey($p[2])) { $matProfs[$p[2]] = @() }
    $matProfs[$p[2]] += $p[0]
}

# ─── 6. Groups + Sessions + build grpMap ─────────────────
Write-Host "Inserting 45 groups and sessions..." -ForegroundColor Cyan
$days      = @("Lundi","Mardi","Mercredi","Jeudi","Samedi","Dimanche")
$timeSlots = @(@("08:00","10:00"),@("10:00","12:00"),@("14:00","16:00"),@("16:00","18:00"))
$annee     = 2026

# grpMap: codegrp -> @{ prof; idmat; codeNiv }
$grpMap  = @{}
$midList = @($matieres.Keys)
$nkList  = @($niveaux.Keys)
$slotIdx = 0

foreach ($mid in $midList) {
    $profPool = if ($matProfs.ContainsKey($mid)) { $matProfs[$mid] } else { @($profList[0][0]) }
    $pi = 0
    foreach ($nk in $nkList) {
        $grpCode = "GRP_${mid}_${nk}"
        $codeNiv = "${mid}_${nk}"
        $prof    = $profPool[$pi % $profPool.Count]; $pi++

        # Insert group
        Exec "INSERT OR IGNORE INTO grp(codegrp,[#idmat],[#codeNiv]) VALUES(@g,@i,@n)" `
             @{"@g"=$grpCode; "@i"=$mid; "@n"=$codeNiv}

        # Store mapping (no DB read needed)
        $grpMap[$grpCode] = @{ prof=$prof; idmat=$mid; codeNiv=$codeNiv }

        # Two sessions per group
        $day1  = $days[$slotIdx % $days.Count]
        $day2  = $days[($slotIdx+2) % $days.Count]
        # Make sure day1 != day2
        if ($day1 -eq $day2) { $day2 = $days[($slotIdx+3) % $days.Count] }
        $s1    = $timeSlots[$slotIdx % $timeSlots.Count]
        $s2    = $timeSlots[($slotIdx+1) % $timeSlots.Count]

        Exec "INSERT OR IGNORE INTO seance([#codegrp],[#annee],[#nomprof],dayy,heureD,heureF) VALUES(@g,@a,@p,@d,@hd,@hf)" `
             @{"@g"=$grpCode;"@a"=$annee;"@p"=$prof;"@d"=$day1;"@hd"=$s1[0];"@hf"=$s1[1]}
        Exec "INSERT OR IGNORE INTO seance([#codegrp],[#annee],[#nomprof],dayy,heureD,heureF) VALUES(@g,@a,@p,@d,@hd,@hf)" `
             @{"@g"=$grpCode;"@a"=$annee;"@p"=$prof;"@d"=$day2;"@hd"=$s2[0];"@hf"=$s2[1]}
        $slotIdx++
    }
}
Write-Host ("Groups: " + $grpMap.Count + " | Sessions: " + ($grpMap.Count * 2)) -ForegroundColor Green

# ─── 7. Students (400) ───────────────────────────────────
Write-Host "`nInserting 400 students..." -ForegroundColor Cyan
$firstNames = @(
    "Adam","Amir","Anas","Ayoub","Bilal","Driss","Hamza","Imad","Karim","Khalid",
    "Mehdi","Mohamed","Mouad","Nassim","Omar","Rachid","Saad","Sami","Taha","Yassine",
    "Fatima","Hafsa","Imane","Kawtar","Khadija","Lamia","Leila","Meryem","Nadia","Nour",
    "Oumaima","Rania","Rim","Safae","Salma","Sara","Soukaina","Widad","Yasmine","Zineb",
    "Abdellah","Achraf","Adil","Amine","Badr","Brahim","Charaf","Fouad","Hicham","Ilyas"
)
$lastNames = @(
    "Alami","Amrani","Benali","Berrada","Bousaid","Cherkaoui","Chraibi","Daoudi","El Fassi",
    "El Mansouri","Filali","Hamdouni","Hassani","Kadiri","Lahlou","Lamrani","Mansouri","Naciri",
    "Ouazzani","Rhazali","Saidi","Sekkat","Tahiri","Tazi","Ziani","Znati","Bensalem","Bouazza",
    "Chaoui","Guessous","Jabri","Jaidi","Belkadi","Benkirane","Bouchaib","Ennaji","Moussaoui"
)
$guardians = @("Père","Mère","Tuteur","Tutrice")
$months    = @("Septembre","Octobre","Novembre","Décembre","Janvier","Février","Mars","Avril","Mai","Juin")
$prices    = @(250,300,350,400)
$frInsc    = @(300,400,450,500)

$rand = New-Object System.Random(42)
$grpKeys = @($grpMap.Keys)  # snapshot of all group codes

$studentList = @()
for ($i = 1; $i -le 400; $i++) {
    $fn   = $firstNames[$rand.Next($firstNames.Count)]
    $ln   = $lastNames[$rand.Next($lastNames.Count)]
    $nom  = "$fn $ln $i"
    $cin  = "AB" + (100000 + $i)
    $qui  = $guardians[$rand.Next($guardians.Count)]
    $tel  = "066" + (1000000 + $rand.Next(9000000))
    $fri  = $frInsc[$rand.Next($frInsc.Count)]
    $dm   = $rand.Next(1,13).ToString("D2")
    $dd   = $rand.Next(1,28).ToString("D2")
    $dy   = $rand.Next(2024,2027)
    $date = "${dy}-${dm}-${dd}"

    Exec "INSERT OR IGNORE INTO inscStd([#cin],qui,nom,tele,frinsc,dateD) VALUES(@c,@q,@n,@t,@f,@d)" `
         @{"@c"=$cin;"@q"=$qui;"@n"=$nom;"@t"=$tel;"@f"=$fri;"@d"=$date}
    $studentList += $nom
}
Write-Host "400 students done." -ForegroundColor Green

# ─── 8. Raff + Pay ───────────────────────────────────────
Write-Host "Inserting Raff assignments and payments..." -ForegroundColor Cyan
$raffCount = 0
$payCount  = 0

# Month index lookup (avoid Array::IndexOf issues)
$monthIndex = @{}
for ($mi = 0; $mi -lt $months.Count; $mi++) { $monthIndex[$months[$mi]] = $mi + 1 }

$grpCount = $grpKeys.Count

# Use a transaction for speed
$tx = $conn.BeginTransaction()
try {
    foreach ($nom in $studentList) {
        # Pick 2 distinct random group indices (simple approach, no Generic.List needed)
        $idx1 = $rand.Next($grpCount)
        $idx2 = $rand.Next($grpCount - 1)
        if ($idx2 -ge $idx1) { $idx2++ }
        $assigned = @($grpKeys[$idx1], $grpKeys[$idx2])

        foreach ($gc in $assigned) {
            $info = $grpMap[$gc]
            if (-not $info) { continue }
            $prof    = $info.prof
            $idmat   = $info.idmat
            $codeNiv = $info.codeNiv

            # Raff
            $cmd = $conn.CreateCommand()
            $cmd.CommandText = "INSERT OR IGNORE INTO Raff([#nom],[#codegrp],annee,[#nomprof]) VALUES(@n,@g,@a,@p)"
            $cmd.Parameters.AddWithValue("@n", $nom)   | Out-Null
            $cmd.Parameters.AddWithValue("@g", $gc)    | Out-Null
            $cmd.Parameters.AddWithValue("@a", $annee) | Out-Null
            $cmd.Parameters.AddWithValue("@p", $prof)  | Out-Null
            $cmd.ExecuteNonQuery() | Out-Null
            $cmd.Dispose()
            $raffCount++

            # Pay: pick 4 distinct random months using Fisher-Yates shuffle on indices
            $mIndices = 0..($months.Count - 1)
            for ($si = $months.Count - 1; $si -gt 0; $si--) {
                $sj = $rand.Next($si + 1)
                $tmp = $mIndices[$si]; $mIndices[$si] = $mIndices[$sj]; $mIndices[$sj] = $tmp
            }
            $paidMonths = $mIndices[0..3] | ForEach-Object { $months[$_] }

            foreach ($mo in $paidMonths) {
                $prix  = $prices[$rand.Next($prices.Count)]
                $moIdx = $monthIndex[$mo].ToString("D2")
                $dpay  = "2026-${moIdx}-05"

                $cmd2 = $conn.CreateCommand()
                $cmd2.CommandText = "INSERT OR IGNORE INTO pay([#nom],[#codegrp],[#nomprof],[#idmat],[#annee],[#codeNiv],datep,monthp,prix) VALUES(@n,@g,@p,@i,@a,@c,@d,@m,@x)"
                $cmd2.Parameters.AddWithValue("@n", $nom)     | Out-Null
                $cmd2.Parameters.AddWithValue("@g", $gc)      | Out-Null
                $cmd2.Parameters.AddWithValue("@p", $prof)    | Out-Null
                $cmd2.Parameters.AddWithValue("@i", $idmat)   | Out-Null
                $cmd2.Parameters.AddWithValue("@a", $annee)   | Out-Null
                $cmd2.Parameters.AddWithValue("@c", $codeNiv) | Out-Null
                $cmd2.Parameters.AddWithValue("@d", $dpay)    | Out-Null
                $cmd2.Parameters.AddWithValue("@m", $mo)      | Out-Null
                $cmd2.Parameters.AddWithValue("@x", $prix)    | Out-Null
                $cmd2.ExecuteNonQuery() | Out-Null
                $cmd2.Dispose()
                $payCount++
            }
        }
    }
    $tx.Commit()
    Write-Host "Transaction committed." -ForegroundColor Green
} catch {
    $tx.Rollback()
    Write-Host "ERROR: $_" -ForegroundColor Red
}

$conn.Close()
$conn.Dispose()

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "   TEST DATA SEEDING COMPLETE!" -ForegroundColor Green
Write-Host "   Subjects    : 15" -ForegroundColor White
Write-Host "   NiveauMat   : 45" -ForegroundColor White
Write-Host "   Professors  : 20" -ForegroundColor White
Write-Host "   Groups      : $($grpMap.Count)" -ForegroundColor White
Write-Host "   Students    : 400" -ForegroundColor White
Write-Host "   Raff rows   : $raffCount" -ForegroundColor White
Write-Host "   Payment rows: $payCount" -ForegroundColor White
Write-Host "============================================" -ForegroundColor Cyan
