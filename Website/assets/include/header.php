<?php
if(session_status() !== PHP_SESSION_ACTIVE) {
    session_start();
}
?>

<nav class="navbar navbar-expand-sm">
    <img src="assets/img/logo.png" class="logo" alt="Logo">
    <a class="navbar-brand" href="index.php" style="font-weight: bold; font-size: 25px">eko-logika</a>
    <button class="navbar-toggler navbar-light" type="button" data-bs-toggle="collapse" data-bs-target="#main-navigation">
        <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="main-navigation">
        <ul class="navbar-nav">
            <li class="nav-item">
                <a class="nav-link" href="index.php">Pradžia</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="popular.php">Populiariausi</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="compare.php">Palygink</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="dokumentacija.php">Dokumentacija</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="apie.php">Apie</a>
            </li>
            <?php
            
            if (isset($_SESSION["token"])){
                echo    '<li class="nav-item">
                            <a class="nav-link" href="user.php">Naudotojas</a>
                        </li>
                        
                        <li class="nav-item">
                            <a class="nav-link" href="Logout.php">Atsijungti</a>
                        </li>';
            }
            else{
                echo    '<li class="nav-item">
                            <a class="nav-link" href="Login.php">Prisijungti</a>
                        </li>';
            }

            ?>
            
        </ul>
    </div>
</nav>