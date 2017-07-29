az group create --name CodeTweet --location eastus
az acs create --orchestrator-type=kubernetes --resource-group CodeTweet --name=myK8sCluster --agent-count=2 --generate-ssh-keys --windows --admin-username azureuser --admin-password myPassword12

az acs kubernetes install-cli
New-Alias kubectl 'C:\Program Files (x86)\kubectl.exe'
az acs kubernetes get-credentials --resource-group=CodeTweet --name=myK8sCluster

kubectl get nodes

kubectl apply -f codetweet-db.yaml
kubectl apply -f codetweet-rabbitmq.yaml

# The following lines are a patch for the current lack of DNS support in Windows nodes. This has already been pushed to Kubernetes master.
# Check for updates here: https://kubernetes.io/docs/getting-started-guides/windows/
$codetweet_db_ip = kubectl get svc codetweet-db -o=jsonpath='{.spec.clusterIP}'
$codetweet_rabbitmq_ip = kubectl get svc codetweet-rabbitmq -o=jsonpath='{.spec.clusterIP}'
(gc codetweet-web.yaml) -replace 'codetweet-db', $codetweet_db_ip -replace 'codetweet-rabbitmq', $codetweet_rabbitmq_ip | Out-File codetweet-web.yaml.tmp
(gc codetweet-worker.yaml) -replace 'codetweet-db', $codetweet_db_ip -replace 'codetweet-rabbitmq', $codetweet_rabbitmq_ip | Out-File codetweet-worker.yaml.tmp

kubectl apply -f codetweet-web.yaml.tmp
kubectl apply -f codetweet-worker.yaml.tmp

kubectl get pods
kubectl get svc

az group delete --name CodeTweet
