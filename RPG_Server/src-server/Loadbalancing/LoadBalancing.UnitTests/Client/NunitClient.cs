
namespace Photon.LoadBalancing.UnitTests.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;

    using ExitGames.Client.Photon;
    using ExitGames.Client.Photon.LoadBalancing;

    using NUnit.Framework;

    using Photon.LoadBalancing.Operations;

    using ErrorCode = ExitGames.Client.Photon.LoadBalancing.ErrorCode;
    using OperationCode = ExitGames.Client.Photon.LoadBalancing.OperationCode;
    using ParameterCode = ExitGames.Client.Photon.LoadBalancing.ParameterCode;

    public class NunitClient : IDisposable
    {
        private readonly TestClient client;

        private readonly int timeOut;

        private readonly string appId;

        public string UserId { get; set; }

        public IPEndPoint RemoteEndPoint { get; private set; }

        public NunitClient(ConnectionProtocol protocol, string appId, int timeOut)
        {
            this.client = new TestClient(protocol);
            this.timeOut = timeOut;
            this.appId = appId;
        }

        #region properties
        public LoadBalancingPeer PhotonClient 
        {
            get 
            { 
                return this.client.PhotonClient; 
            }            
        }

        public Queue<EventData> EventQueue 
        { 
            get 
            { 
                return this.client.EventQueue; 
            } 
        }

        public Queue<OperationResponse> OperationResponseQueue
        {
            get
            {
                return this.client.OperationResponseQueue;
            }
        }

        #endregion properties

        public EventData WaitForEvent(int millisecodsWaitTime)
        {
            return this.client.WaitForEvent(millisecodsWaitTime);
        }

        public EventData WaitForEvent(byte eventCode, int millisecodsWaitTime)
        {
            return this.client.WaitForEvent(eventCode, millisecodsWaitTime);
        }

        public bool TryWaitForEvent(byte eventCode, int millisecodsWaitTime, out EventData eventData)
        {
            try
            {
                eventData = WaitForEvent(eventCode, millisecodsWaitTime);
                return true;
            }
            catch (TimeoutException)
            {
                eventData = null;
                return false;
            }
        }

        public void Connect(string hostName, int port)
        {
            this.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(hostName), port);
            this.client.Connect(hostName, port, appId);
            var result = this.client.WaitForConnect(this.timeOut);
            Assert.IsTrue(result, "Timeout during connect: address={0}:{1}, appId={2}", hostName, port, appId);
        }

        public void Connect(IPEndPoint endpoint)
        {
            this.Connect(endpoint.Address.ToString(), endpoint.Port);
        }

        public void Connect(string address)
        {
            string[] addressParts = address.Split(':');
            var gameServerIp = addressParts[0];
            var gameServerPort = int.Parse(addressParts[1]);

            this.Connect(gameServerIp, gameServerPort);
        }

        public void ConnectAndAuthenticate(string hostName, int port, string userId)
        {
            this.Connect(hostName, port);

            if (userId != null)
            {
                var op = CreateAuthenticateRequest(userId);
                SendRequestAndWaitForResponse(op);
                this.UserId = userId;
            }
        }

        public void ConnectAndAuthenticate(IPEndPoint endpoint, string userId)
        {
            this.ConnectAndAuthenticate(endpoint.Address.ToString(), endpoint.Port, userId);
        }

        public void ConnectAndAuthenticate(string address, string userId)
        {
            this.Connect(address);

            if (userId != null)
            {
                var op = CreateAuthenticateRequest(userId);
                SendRequestAndWaitForResponse(op);
            }
        }

        public void Close()
        {
            this.client.Close();
        }

        public void Dispose()
        {
            this.Close();
        }


        public OperationResponse SendRequestAndWaitForResponse(OperationRequest request, short expectedResult = 0)
        {
            client.SendOperationRequest(request);
            var response = client.WaitForOperationResponse(this.timeOut);
            Assert.AreEqual(request.OperationCode, response.OperationCode);
            if (response.ReturnCode != expectedResult)
            {
                Assert.Fail("Request failed: opCode={0}, expected return code {1} but got returnCode={2}, msg={3}", request.OperationCode, expectedResult, response.ReturnCode, response.DebugMessage);
            }

            return response;
        }

        public CreateGameResponse CreateGame(string gameId, Operations.ErrorCode expectedResult)
        {
            var createGameRequest = new CreateGameRequest { GameId = gameId };
            return this.CreateGame(createGameRequest, expectedResult);
        }

        public CreateGameResponse CreateGame(string gameId, bool isVisible, bool isOpen, byte maxPlayer, Operations.ErrorCode expectedResult)
        {
            var createGameRequest = new CreateGameRequest 
            {
                GameId = gameId,
                GameProperties = new Hashtable()
            };

            createGameRequest.GameProperties[(byte)GameParameter.IsVisible] = isVisible;
            createGameRequest.GameProperties[(byte)GameParameter.IsOpen] = isOpen;
            createGameRequest.GameProperties[(byte)GameParameter.MaxPlayer] = maxPlayer;

            return this.CreateGame(createGameRequest, expectedResult);
        }

        public CreateGameResponse CreateGame(CreateGameRequest createGameRequest, Operations.ErrorCode expectedResult)
        {
            var request = CreateOperationRequest(OperationCode.CreateGame);

            if (createGameRequest.GameId != null)
            {
                request.Parameters[ParameterCode.RoomName] = createGameRequest.GameId;
            }

            if (createGameRequest.LobbyName != null)
            {
                request.Parameters[(byte)Operations.ParameterCode.LobbyName] = createGameRequest.LobbyName;
            }

            if (createGameRequest.LobbyType != 0)
            {
                request.Parameters[(byte)Operations.ParameterCode.LobbyType] = createGameRequest.LobbyType;
            }

            if (createGameRequest.GameProperties != null)
            {
                request.Parameters[(byte)Operations.ParameterCode.GameProperties] = createGameRequest.GameProperties;
            }

            var response = this.SendRequestAndWaitForResponse(request, (short)expectedResult);
            return GetCreateGameResponse(response);
        }

        public OperationResponse LeaveGame()
        {
            var request = new OperationRequest { OperationCode = OperationCode.Leave };
            return this.SendRequestAndWaitForResponse(request);
        }

        public OperationResponse JoinLobby(string lobbyName = null, byte? lobbyType = null)
        {
            var operationRequest = CreateOperationRequest(OperationCode.JoinLobby);
            if (lobbyName != null)
            {
                operationRequest.Parameters[(byte)Operations.ParameterCode.LobbyName] = lobbyName;
            }

            if (lobbyType.HasValue)
            {
                operationRequest.Parameters[(byte)Operations.ParameterCode.LobbyType] = lobbyType.Value;
            }

            return this.SendRequestAndWaitForResponse(operationRequest);
        }

        public JoinGameResponse JoinGame(string gameId, Operations.ErrorCode expectedResult)
        {
            var joinRequest = new JoinGameRequest { GameId = gameId };
            return JoinGame(joinRequest, expectedResult);
        }

        public JoinGameResponse JoinGame(JoinGameRequest joinRequest, Operations.ErrorCode expectedResult)
        {
            var request = CreateOperationRequest((byte)Operations.OperationCode.JoinGame);

            if (joinRequest.GameId != null)
            {
                request.Parameters.Add((byte)Operations.ParameterCode.GameId, joinRequest.GameId);
            }

            if (joinRequest.CreateIfNotExists)
            {
                request.Parameters.Add((byte)Operations.ParameterCode.CreateIfNotExists, joinRequest.CreateIfNotExists);
            }

            var operationResponse = this.SendRequestAndWaitForResponse(request, (short)expectedResult);
            return GetJoinGameResponse(operationResponse);
        }

        public JoinRandomGameResponse JoinRandomGame(JoinRandomGameRequest request, short expectedResult, params string[] expectedRoomNames)
        {
            var operationRequest = new OperationRequest();
            operationRequest.OperationCode = OperationCode.JoinRandomGame;
            operationRequest.Parameters = new Dictionary<byte, object>();

            if (request.GameProperties != null)
            {
                operationRequest.Parameters[ParameterCode.GameProperties] = request.GameProperties;
            }

            if (request.QueryData != null)
            {
                operationRequest.Parameters[(byte)Operations.ParameterCode.Data] = request.QueryData;
            }

            if (request.JoinRandomType != 0)
            {
                operationRequest.Parameters[ParameterCode.MatchMakingType] = request.JoinRandomType;
            }

            if (request.LobbyName != null)
            {
                operationRequest.Parameters[(byte)Operations.ParameterCode.LobbyName] = request.LobbyName;
            }

            if (request.LobbyType != 0)
            {
                operationRequest.Parameters[(byte)Operations.ParameterCode.LobbyType] = request.LobbyType;
            }

            var response = this.SendRequestAndWaitForResponse(operationRequest, expectedResult);
            var joinRandomResponse = GetJoinRandomGameResponse(response);
            if (expectedResult != ErrorCode.Ok)
            {
                return joinRandomResponse;
            }
         
            if (expectedRoomNames == null || expectedRoomNames.Length == 0)
            {
                return joinRandomResponse;
            }

            foreach (var id in expectedRoomNames)
            {
                if (id == joinRandomResponse.GameId)
                {
                    return joinRandomResponse;
                }
            }

            Assert.Fail("Unexpected game on join random: gameId={0}", joinRandomResponse.GameId);
            return joinRandomResponse;
        }

        public OperationResponse JoinRandomGame(Hashtable gameProperties, string query, short expectedResult, string lobbyName = null, byte? lobbyType = null, params string[] expectedRoomNames)
        {
            var operationRequest = new OperationRequest();
            operationRequest.OperationCode = OperationCode.JoinRandomGame;
            operationRequest.Parameters = new Dictionary<byte, object>();

            if (gameProperties != null)
            {
                operationRequest.Parameters[ParameterCode.GameProperties] = gameProperties;
            }

            if (query != null)
            {
                operationRequest.Parameters[(byte)Operations.ParameterCode.Data] = query;
            }

            if (lobbyName != null)
            {
                operationRequest.Parameters[(byte)Operations.ParameterCode.LobbyName] = lobbyName;
            }

            if (lobbyType.HasValue)
            {
                operationRequest.Parameters[(byte)Operations.ParameterCode.LobbyType] = lobbyType.Value;
            }

            var response = this.SendRequestAndWaitForResponse(operationRequest, expectedResult);
            if (expectedResult != ErrorCode.Ok)
            {
                return response;
            }

            string gameId;
            if (!TryGetParameter(response, ParameterCode.RoomName, out gameId))
            {
                Assert.Fail("GameId is missing in join random response");
            }

            if (expectedRoomNames == null || expectedRoomNames.Length == 0)
            {
                return response;
            }

            foreach (var id in expectedRoomNames)
            {
                if (id == gameId)
                {
                    return response;
                }
            }

            Assert.Fail("Unexpected game on join random: gameId={0}", gameId);
            return response;
        }

        public OperationResponse FindFriends(string[] userIds, out bool[] onlineStates, out string[] userStates)
        {
            var request = CreateFindfriendsRequest(userIds);
            var response = this.SendRequestAndWaitForResponse(request, ErrorCode.Ok);

            Assert.IsTrue(TryGetParameter(response, 1, out onlineStates));
            Assert.IsTrue(TryGetParameter(response, 2, out userStates));
            
            Assert.AreEqual(userIds.Length, onlineStates.Length);
            Assert.AreEqual(userIds.Length, userStates.Length);

            return response;
        }

        private static OperationRequest CreateOperationRequest(byte operationCode)
        {
            return new OperationRequest
            {
                OperationCode = operationCode,
                Parameters = new Dictionary<byte, object>()
            };
        }

        private static OperationRequest CreateAuthenticateRequest(string userId)
        {
            var op = CreateOperationRequest(OperationCode.Authenticate);
            op.Parameters.Add(ParameterCode.UserId, userId);
            return op;
        }

        private static OperationRequest CreateFindfriendsRequest(string[] userIdList)
        {
            var op = CreateOperationRequest((byte)Operations.OperationCode.FiendFriends);
            op.Parameters.Add(1, userIdList);
            return op;
        }

        private static CreateGameResponse GetCreateGameResponse(OperationResponse op)
        {
            var res = new CreateGameResponse();
            res.GameId = GetParameter<string>(op, Operations.ParameterCode.GameId, true);
            res.Address = GetParameter<string>(op, Operations.ParameterCode.Address, true);
            return res;
        }

        private static JoinGameResponse GetJoinGameResponse(OperationResponse op)
        {
            var res = new JoinGameResponse();
            res.Address = GetParameter<string>(op, Operations.ParameterCode.Address, true);
            res.NodeId = GetParameter<byte>(op, Operations.ParameterCode.NodeId, true);
            return res;
        }

        private static JoinRandomGameResponse GetJoinRandomGameResponse(OperationResponse op)
        {
            var res = new JoinRandomGameResponse();
            res.GameId = GetParameter<string>(op, Operations.ParameterCode.GameId, false);
            res.Address = GetParameter<string>(op, Operations.ParameterCode.Address, false);
            res.NodeId = GetParameter<byte>(op, Operations.ParameterCode.NodeId, true);
            return res;
        }

        private static bool TryGetParameter<T>(OperationResponse response, byte parameterCode, out T value)
        {
            value = default(T);

            if (response.Parameters == null)
            {
                return false;
            }

            object temp;
            if (response.Parameters.TryGetValue(parameterCode, out temp) == false)
            {
                return false;
            }

            Assert.IsInstanceOf<T>(temp);
            value = (T)temp;
            return true;
        }

        private static T GetParameter<T>(OperationResponse response, Operations.ParameterCode parameterCode, bool isOptional)
        {
            T value;
            if (!TryGetParameter(response, (byte)parameterCode, out value))
            {
                if (isOptional == false && response.ReturnCode == (short)Operations.ErrorCode.Ok)
                {
                    Assert.Fail("Parameter {0} is missing in operation response {1}", parameterCode, (Operations.OperationCode)response.OperationCode);
                }

                return default(T);
            }

            return value;
        }
    }
}
