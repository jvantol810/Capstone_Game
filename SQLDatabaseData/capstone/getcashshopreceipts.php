<?php

    $con = mysqli_connect('localhost', 'root', 'root', 'csc483capstone');
    //Check that the connect was established.
    if (mysqli_connect_errno())
    {
        echo "1: Failed to connect."; // Error code 1 = connection failed.
        exit();
    }
    $userid = $_POST['PLAYERID'];

    if (empty($userid))
    {
        echo "3: No playerid found.";
        exit();
    }

    $purchasesquery = "SELECT playerid, itemid FROM cashshop_purchases WHERE playerid='" . $userid . "';";

    $purchasesresults = mysqli_query($con, $purchasesquery) or die("2: Failed to query cash shop purchase for playerid: " . $userid . "."); //Error code 2 = name check query failed.
    //echo("0" . "\t" . mysqli_num_rows($leaderboardresults) . "\t"); //0 = Success.
    while($row = mysqli_fetch_array($purchasesresults))
    {
        //$output = $output . "/t" . $row['username'] . "/t" . $row['time'];
        echo($row['itemid'] . "\t");
    }
?>