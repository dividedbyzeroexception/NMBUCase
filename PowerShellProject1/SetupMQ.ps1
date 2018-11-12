#
# SetupMQ.ps1
#

#Start MessageQueue
docker run -d --hostname NO_NMBU_IT_MQ --name container_mq -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=pass -e RABBITMQ_DEFAULT_VHOST=vhost -p 4369:4369 -p 5671:5671 -p 5672:5672 -p 15671:15671 -p 15672:15672 rabbitmq:management
#Check its running
docker ps



#Setting Up the Exchange and Queues
cd D:\NMBU\RabbitMQTools

git clone https://github.com/mariuszwojcik/RabbitMQTools.git
Import-Module .\RabbitMQTools.psd1

$mqHost="localhost"
$mqUser="user"
$mqPass="pass"
$mqQueueName = "Person"
$mqExchangeName = "IDM"
$mqRoutingKey = "Person.All"



#If !Exchange, create Exchange 
if(!(Get-RabbitMQExchange -ComputerName $mqHost -UserName $mqUser -Password $mqPass -VirtualHost vhost | where { $_.Name -EQ $mqExchangeName} )){

    Add-RabbitMQExchange -Name $mqExchangeName -ComputerName $mqHost -UserName $mqUser -Password $mqPass -Type topic -VirtualHost vhost
}
else{
    Write-Host "Exchange already exists."
}


#If !Queue, create Queue
if(!(Get-RabbitMQQueue -ComputerName $mqHost -UserName $mqUser -Password $mqPass -VirtualHost vhost | where { $_.Name -EQ $mqQueueName})){

    Add-RabbitMQQueue -Name $mqQueueName -ComputerName $mqHost -UserName $mqUser -Password $mqPass -VirtualHost vhost
}
else{
    Write-Host "Queue already exists."
}



# !Binding, create Binding
if(!(Get-RabbitMQQueueBinding -ComputerName $mqHost -UserName $mqUser -Password $mqPass -name $mqQueueName -VirtualHost vhost | Where-Object {$_.source -eq $mqExchangeName -and $_.destination -EQ $mqQueueName } )){
        
    Add-RabbitMQQueueBinding -ComputerName $mqHost -UserName $mqUser -Password $mqPass -VirtualHost vhost -ExchangeName $mqExchangeName -Name $mqQueueName -RoutingKey $mqRoutingKey
}
else{
    Write-Host "Binding already exists."
}

