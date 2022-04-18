<?php

    $con = mysqli_connect('localhost', 'root', 'root', 'csc483capstone');
    //Check that the connect was established.
    if (mysqli_connect_errno())
    {
        echo "1: Failed to connect."; // Error code 1 = connection failed.
        exit();
    }
    $userid = $_POST['PLAYERID'];
    $itemid = $_POST['ITEMID'];
    $price = $_POST['PRICE'];
    if (empty($userid) || empty($itemid) || empty($price))
    {
        echo "Missing a data row.";
        exit();
    }
    //Check if item already purchases
    $purchasesquery = "SELECT playerid, itemid FROM cashshop_purchases WHERE (playerid='" . $userid . "' AND itemid='" . $itemid . "');";
    $purchasecheck = mysqli_query($con, $purchasesquery);

    if(mysqli_num_rows($purchasecheck) > 0)
    {
        echo "Player already has receipt for this item.";
        exit();
    }

    //Insert receipt
    $insertuserreceipt = "INSERT INTO cashshop_purchases (playerid, itemid, price, date) VALUES('" . $userid . "', '" . $itemid . "', '" . $price . "', '" . date("Y-m-d") ."');";
    mysqli_query($con, $insertuserreceipt) or die("Failed to insert receipt");

    echo "Successfully Inserted";
?>