create_soft_link () {
    realFolder="$1"
    link="$2"

    if [ ! -L "$link" ]
    then
        ln -s "$realFolder" "$link"
        echo $"Created symlink at $realFolder to $link"
    else
        echo "Symlink to $realFolder already exists at $link"
    fi
}

mkdir -p "${PWD}/Assets/Softlinks/MQTTnet/Source/MQTTnet"
create_soft_link "${PWD}/MQTTnet/Source/MQTTnet" "${PWD}/Assets/Softlinks/MQTTnet/Source/MQTTnet"
create_soft_link "${PWD}/MQTTnet/Source/MQTTnet.Extensions.WebSocket4Net" "${PWD}/Assets/Softlinks/MQTTnet/Source/MQTTnet.Extensions.WebSocket4Net"