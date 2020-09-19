#!/bin/bash
address=${1:-pi@192.168.1.100}
echo "**************************************************************"
echo "Started deployment on $address "
echo "**************************************************************"
# remove the artificats locally
echo "remove the artificats locally"
rm -rf bin/release/

echo "restoring packages ..."
dotnet restore
echo "publishing ..."
dotnet publish -c Release -r linux-arm -p:PublishSingleFile=true -p:PublishTrimmed=true

# remove the remote artifacts
echo "deploying to: $address ..."
echo "stopping service on $address"
ssh -i ~/.ssh/id_pi_rsa $address "sudo service HomeDirectorApi stop"
echo "attempting to disable the service on $address"
ssh -i ~/.ssh/id_pi_rsa $address "sudo systemctl disable HomeDirectorApi.service"
echo "attempting to remove the service unit file on $address"
ssh -i ~/.ssh/id_pi_rsa $address "sudo rm /lib/systemd/system/HomeDirectorApi.service"

echo "removing files from $address ..."
ssh -i ~/.ssh/id_pi_rsa $address "rm ~/HomeDirectorApi/ -rf"

echo "copying published files to $address ..."
scp -i ~/.ssh/id_pi_rsa -r bin/release/netcoreapp3.1/linux-arm/publish/ $address:~/HomeDirectorApi/
scp -i ~/.ssh/id_pi_rsa HomeDirectorApi.service $address:~/HomeDirectorApi/

echo "Copying the service files to designated location on $address"
ssh -i ~/.ssh/id_pi_rsa $address sudo cp "/home/pi/HomeDirectorApi/HomeDirectorApi.service" "/lib/systemd/system/HomeDirectorApi.service"

echo "Enabling the service on $address"
ssh -i ~/.ssh/id_pi_rsa $address sudo systemctl daemon-reload
ssh -i ~/.ssh/id_pi_rsa $address sudo systemctl enable HomeDirectorApi.service
echo "Starting the service on $address"
ssh -i ~/.ssh/id_pi_rsa $address sudo systemctl start HomeDirectorApi.service
echo "**************************************************************"
echo "Finished deployment on $address "
echo "**************************************************************"