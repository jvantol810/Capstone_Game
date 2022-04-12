<?php

    $con = mysqli_connect('localhost', 'root', 'root', 'csc483capstone');
    //Check that the connect was established.
    if (mysqli_connect_errno())
    {
        echo "1: Failed to connect."; // Error code 1 = connection failed.
        exit();
    }
    //Connection was established.
    
	//$_post['username'] = 'JohnGoodlaski';
	//$_post['password'] = 'Password';
	
	$username = $_POST['USERNAME'];
	$score = $_POST['SCORE'];
	
	//Establish Count
    $countcheckquery = "SELECT count(username) as 'count' from leaderboard WHERE username='" . $username . "';";
    $countcheck = mysqli_query($con,$countcheckquery) or die("2: Countcheck Query Failed");
	if (mysqli_num_rows($countcheck) > 0)
	{
		$tableinfo = mysqli_fetch_assoc($countcheck);
		$count = $tableinfo["count"];
		
	}
	else
	{
		echo "Failed count";
	}
	
	
	if(empty($username) || empty($score))
	{
		echo "Name or Score is empty";
	}
	else
	{
		//add score to table
		$insertuserquery = "INSERT INTO leaderboard (username, time, count) VALUES('" . $username . "', '" . $score . "', '" . $count . "');";
		mysqli_query($con, $insertuserquery) or die("4: Insert Player Query");
	}
	   
?>