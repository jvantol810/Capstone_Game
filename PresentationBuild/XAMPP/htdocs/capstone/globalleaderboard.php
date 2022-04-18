<?php

    $con = mysqli_connect('localhost', 'root', 'root', 'csc483capstone');
    //Check that the connect was established.
    if (mysqli_connect_errno())
    {
        echo "1: Failed to connect."; // Error code 1 = connection failed.
        exit();
    }
    if (isset($_POST['LOCALFLAG']) && isset($_POST['USERNAME'])) {
        $localflag = $_POST["LOCALFLAG"];
        $filteredusername = $_POST["USERNAME"];
    }
    

    
    $leaderboardquery = "";

    if(empty($localflag))
    { //If empty localflag, it is a global results request
        $leaderboardquery = "SELECT username, time, levelscompleted FROM leaderboard ORDER BY levelscompleted DESC, time;";
    }
    else
    {
        if(empty($filteredusername))
        {
            echo "8: No Local Entries";
            exit();
        }
        else
        {
            $leaderboardquery = "SELECT username, time, levelscompleted FROM leaderboard WHERE username='" . $filteredusername . "';";
        }
        
    }

    

    //Connection was established.
    

    $leaderboardresults = mysqli_query($con, $leaderboardquery) or die("2: Failed to query."); //Error code 2 = name check query failed.

    //echo("0" . "\t" . mysqli_num_rows($leaderboardresults) . "\t"); //0 = Success.
    while($row = mysqli_fetch_array($leaderboardresults))
    {
        //$output = $output . "/t" . $row['username'] . "/t" . $row['time'];
        echo($row['username'] . "\t" . $row['time'] ."\t" . $row['levelscompleted'] . "\t");
    }
?>