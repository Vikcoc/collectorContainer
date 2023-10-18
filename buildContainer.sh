sudo docker image remove eth_collector
dotnet publish -c Release -o app/
sudo docker build -t eth_collector .
sudo docker save -o eth_collector.tar eth_collector
sudo chmod 755 eth_collector.tar