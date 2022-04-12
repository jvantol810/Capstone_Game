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
	$password = $_POST['PASSWORD'];

	
	if(empty($username))
	{
		echo "Name is empty";
	}
	else
	{	
		//Check if name exists
    $namecheckquery = "SELECT username, salt, hash, time from leaderboard WHERE username='" . $username . "';";

    $namecheck = mysqli_query($con,$namecheckquery) or die("2: Namecheck Query Failed");
	

	if (mysqli_num_rows($namecheck) < 1)
	{
		echo "3: Username not found"; //error #3
		exit();
	}

	
	//get login info from query
	$tableinfo = mysqli_fetch_assoc($namecheck);
	$salt = $tableinfo["salt"];
	$hash = $tableinfo["hash"];
	
	$loginhash = crypt($password, $salt);
	if ($hash != $loginhash)
	{
		echo "4: Incorrect Password"; //Error #4
		exit();
	}
	else
	{
		echo $tableinfo["time"];
	}
	
	
    //add user to table
    // $salt = "\$5\$rounds=5000\$" . "steamedhams" . $username . "\$";
    // $hash = crypt($password, $salt);

    // $insertuserquery = "INSERT INTO leaderboard (username, hash, salt) VALUES('" . $username . "', '" . $hash . "', '" . $salt . "');";
    // mysqli_query($con, $insertuserquery) or die("4: Insert Player Query");

    // echo("0: Success Insert");

	}

   
?>