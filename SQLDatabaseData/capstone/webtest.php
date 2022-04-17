<?php

	$con = mysqli_connect('localhost', 'root', 'root', 'csc483capstone');
    //Check that the connect was established.
    if (mysqli_connect_errno())
    {
        echo "1: Failed to connect."; // Error code 1 = connection failed.
        exit();
    }
    //Connection was established.

	
	if(!empty($username))
	{
		echo "Name is empty";
	}
	else
	{	
		//Check if name exists
        $namecheckquery = "SELECT playerid, username, salt, hash from accounts WHERE username='testuser';";

        $namecheck = mysqli_query($con,$namecheckquery) or die("2: Namecheck Query Failed" . mysqli_error($con));
        

        if (mysqli_num_rows($namecheck) != 1)
        {
            echo "3: Username not found"; //error #3
            exit();
        }

        $namecheckvalues = "";
        //get login info from query
        while($row = mysqli_fetch_array($namecheck))
        {
            //$output = $output . "/t" . $row['username'] . "/t" . $row['time'];
            $namecheckvalues = $row['playerid'] . "\t" . $row['username'];
        }

        $tableinfo = mysqli_fetch_assoc($namecheck);
        echo $namecheckvalues;

        
    }
?>