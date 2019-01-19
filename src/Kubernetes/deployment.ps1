az group create --name CodeTweet --location westeurope
az acs create --orchestrator-type=kubernetes --resource-group CodeTweet --name=myK8sCluster --agent-count=2 --generate-ssh-keys --windows --admin-username azureuser --admin-password myPassword12

#az acs kubernetes install-cli
#New-Alias kubectl 'C:\Program Files (x86)\kubectl.exe'
az acs kubernetes get-credentials --resource-group=CodeTweet --name=myK8sCluster

kubectl get nodes

kubectl apply -f codetweet-db.yaml
kubectl apply -f codetweet-rabbitmq.yaml
kubectl apply -f codetweet-web.yaml
kubectl apply -f codetweet-worker.yaml

kubectl get pods
kubectl get svc

az group delete --name CodeTweet
