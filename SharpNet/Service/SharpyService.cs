using System;
using FluentValidation;
using Funq;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using SharpNet.Business.Repl.Strategy;
using SharpNet.Service.Request;
using SharpNet.Service.Response;
using SharpNet.Service.Validation;

namespace SharpNet.Service
{
    public class SharpyService 
        : ServiceStack.ServiceInterface.Service
    {
        /// <summary>
        /// Called when client wishes to eval some C# code
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public EvaluateResponse Post(EvaluateRequest request)
        {
            //validate request
            new EvaluateRequestValidation()
                .ValidateAndThrow(request);

            EvaluateResponse response = null;

            //carry out the strategy to evaluate code
            new EvaluateRequestStrategy(
                request,
                evaluateResponse =>
                {
                    response = evaluateResponse;
                }).Execute();

            //if the response isn't called,
            //something is wrong.
            if(response == null)
                throw new InvalidOperationException(
                    "Response is null"
                    );

            return response;
        }

        /// <summary>
        /// Called when client needs to activate a session.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActivateResponse Post(ActivateRequest request)
        {
            ActivateResponse response = null;


            new ActivateSessionStrategy(
                activateResponse => response = activateResponse
                )
                .Execute();


            if(response == null)
                throw new InvalidOperationException(
                    "Response is null"
                    );

            return response;
        }
    }
}